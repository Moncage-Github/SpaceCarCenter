using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tuning
{
    [RequireComponent(typeof(BlinkObject))]
    public class Parts : MonoBehaviour, IInfoUIShowable
    {
        [field: SerializeField] public PartsData Data { get; protected set; }
        
        public PartsType Type { get => Data.Type; }

        private CapsuleCollider2D _collider;

        private BlinkObject _blinkObject;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<CapsuleCollider2D>();

            _blinkObject = GetComponent<BlinkObject>();
        }

        private void OnEnable()
        {
            PartsPool.Instance.AddObjectAtPool(this);
        }

        private void OnDisable()
        {
            PartsPool.Instance?.RemoveObjectAtPool(this);
        }

        public void Init()
        {
            _renderer.sprite = Data.Sprite;

            UpdateCollider();
        }

        public void Init(PartsData data)
        {
            Data = data;
            Init();
        }

        private void UpdateCollider()
        {
            if (_renderer.sprite == null)
            {
                //Debug.LogWarning("스프라이트가 없습니다!");
                return;
            }

            // 스프라이트의 바운드 정보를 가져옴
            Bounds spriteBounds = _renderer.sprite.bounds;
            Vector3 size = spriteBounds.size;

            // 방향 자동 설정
            if (size.y >= size.x)
            {
                _collider.direction = CapsuleDirection2D.Vertical;
            }
            else
            {
                _collider.direction = CapsuleDirection2D.Horizontal;
            }

            // 콜라이더 오프셋과 크기 설정
            _collider.offset = spriteBounds.center;
            _collider.size = size;
        }

        public void OnSelected()
        {
            _blinkObject.StartBlink();
            var pos = transform.position;
            pos.z = -1;
            transform.position = pos;
        }

        public void OnUnselected()
        {
            _blinkObject.StopBlink();
            var pos = transform.position;
            pos.z = 0;
            transform.position = pos;
        }

        public bool IsMouseEnter(Vector2 mousePos)
        {
            return _collider.OverlapPoint(mousePos);
        }

        public void SetPartsInfo(InfoPanel infoPanel)
        {
            infoPanel.Init();

            infoPanel.SetName(Data.Name);
            infoPanel.SetPartsImage(Data.Sprite);

            infoPanel.SetStat1(Data.Stat1);
            infoPanel.SetStat2(Data.Stat2);
            infoPanel.SetStat3(Data.Stat3);
            infoPanel.SetStat4(Data.Stat4);

            infoPanel.SetQualilty(Data.Quality);
        }
    }
}