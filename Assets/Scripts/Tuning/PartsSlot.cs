using System;
using UnityEngine;

namespace Tuning
{
    public enum PartsType
    {
        Roof,
        Bumper,
        Trunk,
        Frontwindow,
        Door,
        Engine,
        Wheel,
        Custom,
        Exhaust,
    }

    [Serializable]
    public class PartsSlot : MonoBehaviour
    {
        [SerializeField] private PartsType _type;

        [SerializeField] private Parts _parts;

        private Collider2D _collider;

        [SerializeField] private int _layerOrder;
        public int LayerOrder { get => _layerOrder; }

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            _parts = transform.parent.gameObject.GetComponentInChildren<Parts>();
            if (_parts == null) return;

            TryCompositionParts(_parts);
            _parts.Init();
        }

        public bool IsEmpty() => _parts == null;

        public bool TryCompositionParts(Parts parts)
        {
            if (parts.Type != _type)
            {
                Debug.Log("Mismatched Part Types");
                return false;
            }
            _parts = parts;
            _collider.enabled = false;
            _parts.transform.SetParent(transform.parent, true);
            _parts.transform.localPosition = Vector3.zero;
            _parts.CompositeToSlot(this);
            return true;
        }

        public void DecompositionParts()
        {
            _parts = null;
            _collider.enabled = true;
        }


    }
}