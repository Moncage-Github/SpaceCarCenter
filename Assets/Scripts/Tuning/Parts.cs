using UnityEngine;

namespace Tuning
{
    public class PartsStat 
    {
        public int Stat1;
        public int Stat2;
        public int Stat3;
        public int Stat4;

        public PartsStat(int stat1, int stat2, int stat3, int stat4)
        {
            Stat1 = stat1;
            Stat2 = stat2;
            Stat3 = stat3;
            Stat4 = stat4;
        }
    }

    public class Parts : PartsBase
    {
        public readonly PartsStat Stat = new(10, 5, 3, 4);
        public enum State { Dropped, PickUped, Composed, ScrewComposed };
        public bool NeedsScrewTightening;

        private readonly int _maxScrewTightenCount = 4;
        private int _curScrewTightenCount = 0;

        [SerializeField] private PartsType _type;
        public PartsType Type { get => _type; private set => _type = value; }

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

        private void Start()
        {
            Quality = Random.Range(0, 80);
        }

        public void Init()
        {
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
            Renderer.sortingOrder = slot.LayerOrder - 1;
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