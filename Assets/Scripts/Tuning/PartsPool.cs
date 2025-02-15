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
        [SerializeField] private List<PartsBase> _overlapParts = new List<PartsBase>();

        public int OverlapPartsCount => _overlapParts.Count;

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
            if (_overlapParts.Contains(parts)) return;

            parts.Select = true;

            _overlapParts.Add(parts);
            _infoUI.AddPartsInfo(parts);

            if (OverlapPartsCount == 1)
            {
                _infoUI.ShowPanel(parts);   
            }
        }

        public void RemoveOverlapedParts(PartsBase parts)
        {
            if (!_overlapParts.Contains(parts)) return;

            parts.Select = false;

            _overlapParts.Remove(parts);
            _infoUI.RemovePartsInfo(parts);

            if(OverlapPartsCount == 0)
            {
                _infoUI.DeInit();
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