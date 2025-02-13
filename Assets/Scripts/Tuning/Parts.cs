using UnityEngine;

namespace Tuning
{
    public class Parts : PartsBase
    {
        public enum State { Dropped, PickUped, Composed, ScrewComposed };
        public bool NeedsScrewTightening;

        private readonly int _maxScrewTightenCount = 4;
        private int _curScrewTightenCount = 0;

        public PartsType Type;

        private PartsSlot _slot;

        private int _quality;
        public int Quality
        {
            get => _quality;
            private set
            {
                _quality = Mathf.Clamp(value, 0, 100);
            }
        }

        [SerializeField] private Screw _screw;

        //States
        public State CurState;


        public void Init()
        {
            Quality = Random.Range(0, 80);

            if (NeedsScrewTightening)
            {
                CurState = State.ScrewComposed;
                _curScrewTightenCount = _maxScrewTightenCount;
                _screw.Pickup();
                _screw.transform.SetParent(transform);
                _screw.gameObject.SetActive(false);
            }
            else
            {
                CurState = State.Composed;
            }
        }

        public override void Drop()
        {
            CurState = State.Dropped;
            base.Drop();
        }

        public override void Pickup()
        {
            CurState = State.PickUped;
            base.Pickup();
        }

        public void DeCompositeFromSlot()
        {
            GetSlot().DecompositionParts();
            SetSlot(null);
            Pickup();
        }

        public void CompositeToSlot(PartsSlot slot)
        {
            Renderer.sortingOrder = 0;
            CurState = State.Composed;
            SetSlot(slot);
        }

        public void SetScrew(Screw screw)
        {
            if (!NeedsScrewTightening) return;

            _screw = screw;

            if (_screw == null) return;

            _screw.transform.SetParent(transform);
            _screw.transform.localPosition = Vector3.zero;
        }

        public void TryTightenScrew()
        {
            CurState = State.ScrewComposed;

            _curScrewTightenCount++;
            if (_curScrewTightenCount >= _maxScrewTightenCount)
            {
                _screw.gameObject.SetActive(false);
                _curScrewTightenCount = _maxScrewTightenCount;
                Debug.Log("Screw Tighten Finish");
            }
        }

        public void UnSrcew()
        {
            _screw.gameObject.SetActive(true);

            _curScrewTightenCount--;
            if (_curScrewTightenCount <= 0)
            {
                CurState = State.Composed;
                _screw.Drop();
                _screw.gameObject.SetActive(true);
                _screw.transform.position = transform.position;
                _screw.transform.parent = null;
                SetScrew(null);
            }
        }

        public void FixParts(int amount) => Quality += amount;

        private void SetSlot(PartsSlot slot)
        {
            _slot = slot;
        }
        public PartsSlot GetSlot() { return _slot; }
    }
}