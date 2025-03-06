using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Tuning
{
    public class PartsPool : MonoBehaviour
    {
        public static PartsPool Instance;

        [SerializeField] private List<PartsBase> _partsList = new List<PartsBase>();

        [SerializeField] private PartsInfoUI _infoUI;

        public PartsBase SelectedParts => _infoUI.GetSelectedParts();

        public bool PickUped;

        private void Awake()
        {
            Instance = this;
        }

        public void AddPartsAtPool(PartsBase parts)
        {
            _partsList.Add(parts);
        }

        public void RemovePartsAtPool(PartsBase parts)
        {
            if (_partsList.Contains(parts))
                _partsList.Remove(parts);
        }

        public void AddOverlapedParts(PartsBase parts)
        {
            _infoUI.AddPartsInfo(parts);
        }

        public void RemoveOverlapedParts(PartsBase parts)
        {
            _infoUI.RemovePartsInfo(parts);
        }

        public void WheelInput(int input)
        {
            _infoUI.WheelInput(-input);
        }

        private void Update()
        {
            UpadateOverlapedParts();
        }

        private void UpadateOverlapedParts()
        {

            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            foreach (var parts in _partsList)
            {
                if (parts.Collider.OverlapPoint(mousePos))
                {
                    AddOverlapedParts(parts);
                }
                else
                {
                    RemoveOverlapedParts(parts);
                }
            }
        }


        private void OnDestroy()
        {
            Instance = null;
        }
    }
}