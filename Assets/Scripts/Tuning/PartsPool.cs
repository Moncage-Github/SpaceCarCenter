using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Tuning
{
    public class PartsPool : MonoBehaviour
    {
        private static PartsPool _instance;
        public static PartsPool Instance 
        { 
            get
            {
                if(_instance == null)
                    _instance = FindObjectOfType<PartsPool>();
                return _instance;
            }
        }

        [SerializeField] private List<IInfoUIShowable> _objectList = new();

        [SerializeField] private List<PartsSlot> _slotList = new List<PartsSlot>();

        [SerializeField] private PartsInfoUI _infoUI;

        [SerializeField] private GameObject _partsPrefab;
        [SerializeField] private GameObject _screwPrefab;

        private bool _pickUp;
        public IInfoUIShowable SelectedObject { get => _infoUI.GetSelectedObject; }

        public bool PickUped;

        private void OnDestroy()
        {
            _instance = null;
        }

        public void AddObjectAtPool(IInfoUIShowable obj)
        {
            if (!_objectList.Contains(obj))
                _objectList.Add(obj);
        }

        public void RemoveObjectAtPool(IInfoUIShowable obj)
        {
            if (_objectList.Contains(obj))
                _objectList.Remove(obj);
        }

        public void AddSlotAtPool(PartsSlot slot)
        {
            if (!_slotList.Contains(slot))
                _slotList.Add(slot); 
        }

        public void RemoveSlotAtPool(PartsSlot slot)
        {
            if (_slotList.Contains(slot))
                _slotList.Remove(slot);
        }

        public void AddOverlapedParts(IInfoUIShowable obj)
        {
            _infoUI.AddPartsInfo(obj);
        }

        public void RemoveOverlapedParts(IInfoUIShowable obj)
        {
            _infoUI.RemovePartsInfo(obj);
        }

        public void EquipPartsAtSlot(PartsSlot slot, PartsData data)
        {
            slot.EquipParts(data);
            DropParts();
            AddObjectAtPool(slot);
        }

        public PartsData UnEquipPartsAtSlot(PartsSlot slot)
        {
            RemoveObjectAtPool(slot);
            var data = slot.UnequipParts();
            return data;
        }

        private void EnableEquipPreviewSlot(PartsData data)
        {
            foreach(var slot in _slotList)
            {
                if (slot.Type == data.Type)
                {
                    slot.EnableEquipPreview(data.Sprite);
                }
            }
        }

        private void DisableEquipPreviewSlot()
        {
            foreach (var slot in _slotList)
            {
                if (slot.IsInPreviewMode)
                {
                    slot.DisableEquipPreview();
                }
            }
        }

        private void EnableScrewPreviewSlot()
        {
            foreach (var slot in _slotList)
            {
                if (slot.IsInPreviewMode || !slot.HasParts) continue;

                slot.EnableScrewPreview();
            }
        }

        private void DisableScrewPreviewSlot()
        {
            foreach (var slot in _slotList)
            {
                if (slot.IsInPreviewMode)
                {
                    slot.DisableScrewPreview();
                }
            }
        }

        public PartsSlot GetOverlapedSlot()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            for (int i = 0; i < _slotList.Count; i++)
            {
                if (_slotList[i].IsMouseEnter(mousePos))
                {

                    return _slotList[i];
                }
            }
            return null;
        }

        public List<PartsSlot> GetAllOverlapedSlots()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            List <PartsSlot> slots = new List<PartsSlot>();
            for (int i = 0; i < _slotList.Count; i++)
            {
                if (_slotList[i].IsMouseEnter(mousePos))
                {
                    slots.Add(_slotList[i]);
                }
            }

            return slots;
        }

        public void WheelInput(int input)
        {
            if (_pickUp) return;
            _infoUI.WheelInput(-input);
        }

        public void PickUpParts(PartsData data)
        {
            _pickUp = true; 
            _infoUI.PickUpParts();
            EnableEquipPreviewSlot(data);
        }
        public void DropParts()
        {
            _pickUp = false;
            _infoUI.DeInit();
            DisableEquipPreviewSlot();
        }

        public void PickUpScrew()
        {
            _pickUp = true;
            _infoUI.PickUpParts();
            EnableScrewPreviewSlot();
        }

        public void DropScrwe()
        {
            _pickUp = false;
            _infoUI.DeInit();
            DisableScrewPreviewSlot();
        }

        public Parts CreateParts(PartsData data)
        {
            Parts parts = Instantiate(_partsPrefab).GetComponent<Parts>();
            parts.Init(data);

            return parts;
        }

        public Screw CreateScrew()
        {
            Screw screw = Instantiate(_screwPrefab).GetComponent<Screw>();

            return screw;
        }

        private void Update()
        {
            if (_pickUp)
            { 
                     
            }
            else
            {
                UpadateOverlapedParts();
            }
        }

        private void UpadateOverlapedParts()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            foreach (var obj in _objectList)
            {
                if (obj.IsMouseEnter(mousePos))
                {
                    AddOverlapedParts(obj);
                }
                else
                {
                    RemoveOverlapedParts(obj);
                }
            }
        }

        public void ResetUI()
        {
            _infoUI.ResetUI();
            UpadateOverlapedParts();
        }
    }
}