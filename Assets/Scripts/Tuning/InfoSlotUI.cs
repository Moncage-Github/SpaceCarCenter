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

        public IInfoUIShowable Object { get; private set; }
        
        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                if (value)
                {
                    _slotBack.color = Color.white;
                    Object.OnSelected();

                }
                else
                {
                    _slotBack.color = Color.gray;
                    Object.OnUnselected();
                }
            }
        }

        public void Init(IInfoUIShowable obj)
        {
            gameObject.SetActive(true);
            Object = obj;
        }

        public void DeInit()
        {
            Object.OnUnselected();
            Destroy(gameObject);
        }
    }
}