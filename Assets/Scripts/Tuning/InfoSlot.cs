using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class InfoSlot : MonoBehaviour
    {
        [SerializeField] private Image _partsImage;
        [SerializeField] private Image _slotBack;

        private bool _enable;
        public bool Enable
        {
            get => _enable;
            set
            {
                if (value)
                {
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}