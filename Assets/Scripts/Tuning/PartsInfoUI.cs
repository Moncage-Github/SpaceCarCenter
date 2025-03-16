using System.Collections;
using System.Collections.Generic;
using Tuning;
using UnityEngine;
using UnityEngine.UI;

public class PartsInfoUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private InfoPanel _infoPanel;

    [Space(3.0f)]
    [Header("Slot")]
    [SerializeField] private Transform _slotLayout;
    [SerializeField] private List<InfoSlotUI> _slots;
    [SerializeField] private GameObject _slotsPrefab;

    private int _selectedPartsIndex;
    public IInfoUIShowable GetSelectedObject 
    {
        get
        {
            if (_slots.Count > 0) return _slots[_selectedPartsIndex].Object;
            else return null;
        }
    }

    public void ResetUI()
    {
        DeInit();
        foreach (var slot in _slots)
        {
            slot.DeInit();
        }
        _slots.Clear();
    }

    public void DeInit()
    {
        _infoPanel.gameObject.SetActive(false);
        _slotLayout.gameObject.SetActive(true);
        _selectedPartsIndex = 0;
    }

    public void AddPartsInfo(IInfoUIShowable obj)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].Object == obj)
            {
                return;
            }
        }

        var slot = Instantiate(_slotsPrefab, _slotLayout).GetComponent<InfoSlotUI>();
        _slots.Add(slot);

        slot.Init(obj);

        if (_slots.Count == 1)
        {
            _infoPanel.gameObject.SetActive(true);

            _selectedPartsIndex = 0;
            _slots[_selectedPartsIndex].Selected = true;

            obj.SetPartsInfo(_infoPanel);
        }
    }

    public void RemovePartsInfo(IInfoUIShowable parts)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].Object == parts)
            {
                _slots[i].DeInit();

                _slots.RemoveAt(i);

                if (_slots.Count == 0)
                {
                    DeInit();

                    return;
                }

                if (i == _selectedPartsIndex || _selectedPartsIndex > _slots.Count)
                {
                    _selectedPartsIndex = 0;
                    _slots[_selectedPartsIndex].Selected = true;
                    //SetPartsInfo(parts.Data);
                }

                return;
            }
        }
    }

    public void WheelInput(int value)
    {
        if (_slots.Count == 0) return;

        int index = Mathf.Clamp(_selectedPartsIndex + value, 0, _slots.Count - 1);
        if (index == _selectedPartsIndex) return;

        _slots[_selectedPartsIndex].Selected = false;
        _slots[index].Selected = true;

        _selectedPartsIndex = index;
        _slots[_selectedPartsIndex].Object.SetPartsInfo(_infoPanel);
    }

    public void PickUpParts()
    {
        _slotLayout.gameObject.SetActive(false);

        foreach(var slot in _slots)
        {
            slot.DeInit();
        }
        _slots.Clear();
    }

}
