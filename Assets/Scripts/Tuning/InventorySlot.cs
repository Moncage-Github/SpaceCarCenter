using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Tuning
{
    [RequireComponent(typeof(Image))]
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private bool _selected;

        [SerializeField] private ToolType _type;

        public ToolType Type { get => _type; }
        public bool Selected
        {
            get => _selected;
            set 
            {
                _selected = value;
                SetSelected(value);
            }
        }

        private void SetSelected(bool selected)
        {
            var color = _image.color;

            if (selected)
            {
                color.a = 1;
            }
            else
            {
                color.a = 0.2f;
            }

            _image.color = color;
        }
    }
}