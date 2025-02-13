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

    public class PartsSlot : InteractionObject
    {
        [SerializeField] private PartsType _type;

        [SerializeField] private Parts _parts;

        private Collider2D _collider;


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
            _collider.enabled = false;
            parts.transform.SetParent(transform.parent, true);
            parts.transform.localPosition = Vector3.zero;
            parts.CompositeToSlot(this);
            return true;
        }

        public void DecompositionParts()
        {
            _parts = null;
            _collider.enabled = true;
        }
    }
}