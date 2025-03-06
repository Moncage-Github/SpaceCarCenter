using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class InfoSlotUI : MonoBehaviour
    {
        [SerializeField] private Image _partsImage;
        [SerializeField] private Image _slotBack;

        public PartsBase Parts { get; private set; }
        
        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                if (value)
                {
                    _slotBack.color = Color.white;
                    Parts.Select = true;

                }
                else
                {
                    _slotBack.color = Color.gray;
                    Parts.Select = false;

                }
            }
        }

        public void Init(PartsBase parts)
        {
            gameObject.SetActive(true);
            Parts = parts;
        }

        public void DeInit()
        {
            Parts.Select = false;
            Destroy(gameObject);
        }
    }
}