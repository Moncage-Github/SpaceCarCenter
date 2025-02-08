using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TuningHammerMiniGame : MonoBehaviour
{
    private float _barLength;

    [Header("Bar")]
    [SerializeField] private Image _bar;
    [SerializeField] private GameObject _zone;
    [SerializeField] private Image _redZoneImage;
    [SerializeField] private Image _yellowZoneImage;
    [SerializeField] private Image _blueZoneImage;
    [SerializeField][Range(0, 100)] private float _blueZonePercent;
    [SerializeField][Range(0, 100)] private float _yellowZonePercent;
    [SerializeField][Range(0, 100)] private float _redZonePercent;
    private float _barRandomPos;

    [Space(3.0f)]
    [Header("Cusor")]
    [SerializeField] private Image _cursorImage;
    [SerializeField] private float _cursorSpeed;
    [SerializeField] private float _cursorMaxX;
    private bool _moveCursor = false;
    private int _cursorDirection = 1; // 1 : ¿Þ -> ¿À 2 : ¿À -> ¿Þ

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        var barScale = _bar.rectTransform.rect.size;

        _redZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barScale.x * (_redZonePercent / 100));
        _redZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, barScale.y);

        _yellowZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barScale.x * (_yellowZonePercent / 100));
        _yellowZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, barScale.y);

        _blueZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barScale.x * (_blueZonePercent / 100));
        _blueZoneImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, barScale.y);

        _zone.transform.localPosition = new Vector3(_barRandomPos, _zone.transform.localPosition.y);

        Invoke("StartGame", 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCursor();
    }

    private void MoveCursor()
    {
        if (!_moveCursor) return;
        var position =_cursorImage.transform.localPosition;
        position.x += _cursorSpeed * _cursorDirection * Time.fixedDeltaTime;
        position.x = Mathf.Clamp(position.x, -_cursorMaxX, _cursorMaxX);
        _cursorImage.transform.localPosition = position;

        if (_cursorMaxX == position.x || -_cursorMaxX == position.x)
        {
            _cursorDirection *= -1;
        }
    }

    public void StartGame()
    {
        _moveCursor = true;

    }

    public void StopCursor()
    {
        _moveCursor = false;
    }
}
