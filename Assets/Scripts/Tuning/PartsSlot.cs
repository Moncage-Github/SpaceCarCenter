using System;
using TMPro;
using UnityEngine;

namespace Tuning
{
    [RequireComponent(typeof(BlinkObject))]
    public class PartsSlot : MonoBehaviour, IInfoUIShowable
    {
        [field: SerializeField] public PartsType Type { get; private set; }

        [SerializeField] private int _layerOrder;
        public int LayerOrder { get => _layerOrder; }

        [Space(1.0f)]
        [Header("Components")]
        private SpriteRenderer _renderer;
        [SerializeField] private Collider2D _interactionArea;
        private BlinkObject _blinkObject;

        private PartsData _data = null;
        public PartsData Data => _data;

        private bool _hasScrew = false;
        public bool HasScrew => _hasScrew;
        private int _screwTightenCount;


        public bool HasParts { get; private set; }

        public bool IsInPreviewMode { get; private set; }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _blinkObject = GetComponent<BlinkObject>();
        }

        private void OnEnable()
        {
            PartsPool.Instance.AddSlotAtPool(this);   
        }

        public void EnableEquipPreview(Sprite sprite)
        {
            if(HasParts) return;

            IsInPreviewMode = true;
            _renderer.sprite = sprite; 
            var color = _renderer.color;
            color.a = 0.5f;
            _renderer.color = color;
        }

        public void DisableEquipPreview()
        {
            if (!IsInPreviewMode) return;
            if (HasParts) return;

            IsInPreviewMode = false;
            _renderer.sprite = null;
            var color = _renderer.color;
            color.a = 1f;
            _renderer.color = color;
        }

        public void EnableScrewPreview()
        {
            if (Data.NeedScrew && HasScrew == false)
            {
                IsInPreviewMode = true;
                _blinkObject.StartBlink();
            }
        }

        public void DisableScrewPreview()
        {
            IsInPreviewMode = false;
            _blinkObject.StopBlink();
        }

        public void EquipParts(PartsData data)
        {
            if (HasParts) return;

            HasParts = true;
            _data = data;      

            _renderer.color = Color.white;
            IsInPreviewMode = false;

        }

        public PartsData UnequipParts()
        {
            if(!HasParts) return null;

            HasParts = false;   
            var data = Data;
            _renderer.sprite = null;
            _data = null;
            return data;
        }

        public void EquipScrew()
        {
            if (HasScrew) return;

            _hasScrew = true;

            _screwTightenCount = 1;

            Debug.Log("EquipSrew");
        }

        public void TightenScrew()
        {
            if (_screwTightenCount == Data.MaxScrewTightenCount) return;
            _screwTightenCount++;
            Debug.Log($"TightenScrew {_screwTightenCount}");

        }

        public void UntightenScrew()
        {
            _screwTightenCount--;
            Debug.Log($"UntightenScrew {_screwTightenCount}");

            if (_screwTightenCount == 0)
            {
                var screw = PartsPool.Instance.CreateScrew();
                screw.transform.position = transform.position;
                _hasScrew = false;
                    
            }
        }

        public void OnSelected()
        {
            _blinkObject.StartBlink();
            _renderer.sortingOrder = 0;
            var pos = transform.position;
            pos.z = -1f;
            transform.position = pos;
        }

        public void OnUnselected()
        {
            _blinkObject.StopBlink();
            _renderer.sortingOrder = _layerOrder;
            var pos = transform.position;
            pos.z = 0f;
            transform.position = pos;
        }

        public bool IsMouseEnter(Vector2 mousePos)
        {
            return _interactionArea.OverlapPoint(mousePos);
        }

        public void SetPartsInfo(InfoPanel infoPanel)
        {
            infoPanel.Init();

            infoPanel.SetName($"{Data.Name} (Slot)");
            infoPanel.SetPartsImage(Data.Sprite);

            infoPanel.SetStat1(Data.Stat1);
            infoPanel.SetStat2(Data.Stat2);
            infoPanel.SetStat3(Data.Stat3);
            infoPanel.SetStat4(Data.Stat4);

            infoPanel.SetQualilty(Data.Quality);
        }
    }
}