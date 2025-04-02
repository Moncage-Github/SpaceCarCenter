using System;
using System.Collections;
using System.Collections.Generic;
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
    public class PartsData
    {
        public Sprite Sprite;

        public string Name;

        public PartsType Type;

        public int Stat1;
        public int Stat2;
        public int Stat3;
        public int Stat4;

        [Space(5.0f)]
        [Header("Screw")]
        public bool NeedScrew;
        [SerializeField]private int _maxScrewTightenCount = 4;
        public int MaxScrewTightenCount { get => _maxScrewTightenCount; }

        private int _quality;
        public int Quality
        {
            get => _quality;
            private set
            {
                _quality = Mathf.Clamp(value, 0, 100);
            }
        }

        public void FixParts(int amount)
        {
            Quality += amount;
        }
    }
}