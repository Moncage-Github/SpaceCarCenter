using System.Collections;
using Tuning;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Tuning
{
    public class HammerMiniGame : MonoBehaviour
    {
        [Header("Bar")]
        [SerializeField] private Image _bar;
        [SerializeField] private RectTransform _zone;
        [SerializeField] private Image _redZoneImage;
        [SerializeField] private Image _yellowZoneImage;
        [SerializeField] private Image _blueZoneImage;
        [SerializeField][Range(0, 100)] private float _blueZonePercent;
        [SerializeField][Range(0, 100)] private float _yellowZonePercent;
        [SerializeField][Range(0, 100)] private float _redZonePercent;
        private float _zoneStartPercent;

        [Space(3.0f)]
        [Header("Cusor")]
        [SerializeField] private Image _cursorImage;
        [SerializeField] private float _cursorSpeed;
        private float _cursorMaxX;
        private bool _moveCursor = false;
        private int _cursorDirection = 1; // 1 : 왼 -> 오 2 : 오 -> 왼

        [Space(3.0f)]
        [Header("QualityBar")]
        [SerializeField] private Image _qualityBarImage;


        private Parts _partsRef;

        public void Init(Parts parts)
        {
            _partsRef = parts;
            _qualityBarImage.fillAmount = _partsRef.Quality / 100.0f;

            _cursorImage.rectTransform.anchoredPosition = new Vector2(0, 0);

            var barScale = _bar.rectTransform.rect.size;

            _redZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barScale.x * (_redZonePercent / 100));
            _redZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, barScale.y);

            _yellowZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barScale.x * (_yellowZonePercent / 100));
            _yellowZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, barScale.y);

            _blueZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barScale.x * (_blueZonePercent / 100));
            _blueZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, barScale.y);

            _zoneStartPercent = Random.Range(10f, 100f - _blueZonePercent);
            var barPos = _zone.anchoredPosition;
            barPos.x = _bar.rectTransform.rect.width * (_zoneStartPercent / 100);
            _zone.anchoredPosition = barPos;

            _cursorMaxX = _bar.rectTransform.rect.width;

            Invoke("StartGame", 0.25f);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            StopCursor();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveCursor();
        }

        private void MoveCursor()
        {
            if (!_moveCursor) return;
            var position =_cursorImage.rectTransform.anchoredPosition;
            position.x += _cursorSpeed * _cursorDirection * Time.fixedDeltaTime;
            position.x = Mathf.Clamp(position.x, 0, _cursorMaxX);
            _cursorImage.rectTransform.anchoredPosition = position;
            if (position.x == 0 || position.x == _cursorMaxX)
            {
                _cursorDirection *= -1;
            }
        }


        private void StartGame()
        {
            _moveCursor = true;

        }

        private void FinishGame()
        {
            gameObject.SetActive(false);
            TuningPlayer.Instance.FinishHammerGame();
        }

        private void StopCursor()
        {
            if (_moveCursor == false) return;

            _moveCursor = false;

            CheckZone();
        }

        private void CheckZone()
        {
            float cursorPos = _cursorImage.rectTransform.anchoredPosition.x;
            float width = _bar.rectTransform.rect.width;
            float cursorPercent = (cursorPos / width) * 100;
            float centor = _zoneStartPercent + _blueZonePercent / 2;
            int fixAmount = 0;

            if (cursorPercent <= _zoneStartPercent || cursorPercent >= _zoneStartPercent + _blueZonePercent)
            {
                Debug.Log("Fail");
                Invoke("FinishGame", 0.3f);
                return;
            }

            if (cursorPercent >= centor - _yellowZonePercent / 2 && cursorPercent <= centor + _yellowZonePercent / 2)
            {
                if (cursorPercent >= centor - _redZonePercent / 2 && cursorPercent <= centor + _redZonePercent / 2)
                {
                    fixAmount = 100;
                    Debug.Log("Red");
                }
                else
                {
                    fixAmount = 50;
                    Debug.Log("Yellow");
                }
            }
            else
            {
                fixAmount = 20;
                Debug.Log("Blue");
            }

            FixParts(fixAmount);
        }

        private void FixParts(int amount)
        {
            float fillAmount = (_partsRef.Quality + amount) / 100.0f;
            StartCoroutine(ChangeFillAmount(_qualityBarImage, fillAmount, 0.5f));
            _partsRef.FixParts(amount);
        }

        private IEnumerator ChangeFillAmount(Image img, float target, float time)
        {
            float startFill = img.fillAmount; // 시작값 저장
            float elapsed = 0f;

            while (elapsed < time)
            {
                // 현재 시간에 따라 선형 보간(Lerp)하여 fillAmount 값 계산
                img.fillAmount = Mathf.Lerp(startFill, target, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;  // 다음 프레임까지 대기
            }

            // 시간이 다 된 후 정확하게 목표 값으로 설정
            img.fillAmount = target;

            yield return new WaitForSeconds(0.25f);
            FinishGame();
        }
    }
}