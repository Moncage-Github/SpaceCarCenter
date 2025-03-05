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

        private PartsBase _selectedParts = null;
        [SerializeField] private List<PartsBase> _partsList = new List<PartsBase>();
        [SerializeField] private List<PartsBase> _overlapPartsList = new List<PartsBase>();

        public int OverlapPartsCount => _overlapPartsList.Count;


        [SerializeField] private PartsInfoUI _infoUI;

        private void Awake()
        {
            Instance = this;
        }

        public void AddParts(PartsBase parts)
        {
            _partsList.Add(parts);
        }

        public void RemoveParts(PartsBase parts)
        {
            if (_partsList.Contains(parts))
                _partsList.Remove(parts);
        }

        public void AddOverlapedParts(PartsBase parts)
        {
            if (_overlapPartsList.Contains(parts)) return;


            if (OverlapPartsCount == 0)
            {
                _infoUI.ShowPanel(parts);
                parts.Select = true;
                _selectedParts = parts;
            }

            _overlapPartsList.Add(parts);
            _infoUI.AddPartsInfo(parts);
        }

        public void RemoveOverlapedParts(PartsBase parts)
        {
            if (!_overlapPartsList.Contains(parts)) return;

            parts.Select = false;

            _overlapPartsList.Remove(parts);
            _infoUI.RemovePartsInfo(parts);

            if(parts == _selectedParts)
            {
                _selectedParts = null;

                if (OverlapPartsCount == 0)
                {
                    _infoUI.DeInit();
                    parts.Select = false;
                }
                else
                {
                    _selectedParts = _overlapPartsList[0];
                    _selectedParts.Select = true;
                }
            }
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