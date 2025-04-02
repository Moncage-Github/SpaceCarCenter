using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public enum ToolType
    {
        Hand,
        ScrewDriver,
        Hammer,
        Paint
    }

    public class ToolInventory : MonoBehaviour
    {
        [SerializeField] private List<InventorySlot> _invenSlots;

        private InventorySlot _selectedSlot;
        private int _curSlotNum;
        public int SlotNum 
        {
            get => _curSlotNum; 
            set
            {
                if(_selectedSlot !=  null)
                    _selectedSlot.Selected = false;

                _curSlotNum = Mathf.Clamp(value, 1, _invenSlots.Count);
                _selectedSlot = _invenSlots[SlotNum - 1];
                _selectedSlot.Selected = true;
            }
        }
        

        public void Awake()
        {
            SlotNum = 1;
        }

        public ToolType CurTool => _selectedSlot.Type;
    }
}