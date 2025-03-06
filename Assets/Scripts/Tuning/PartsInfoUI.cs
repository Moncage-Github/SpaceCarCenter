using System.Collections;
using System.Collections.Generic;
using Tuning;
using UnityEngine;
using UnityEngine.UI;

public class PartsInfoUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject _partsPanel;
    [SerializeField] private GameObject _screwPanel;

    [Space(3.0f)]
    [Header("Name")]
    [SerializeField] private Text _nameLabel;

    [Space(3.0f)]
    [Header("Stat")]
    [SerializeField] private Text _stat1Label;
    [SerializeField] private Text _stat2Label;
    [SerializeField] private Text _stat3Label;
    [SerializeField] private Text _stat4Label;

    [Space(3.0f)]
    [Header("Image")]
    [SerializeField] private Image _partsImage;

    [Space(3.0f)]
    [Header("Quality")]
    [SerializeField] private Image _barImage;

    [Space(3.0f)]
    [Header("Slot")]
    [SerializeField] private Transform _slotLayout;
    [SerializeField] private List<InfoSlotUI> _slots;
    [SerializeField] private GameObject _slotsPrefab;

    private int _selectedPartsIndex;


    public PartsBase GetSelectedParts()
    {
        if( _selectedPartsIndex > _slots.Count - 1)
            return null;

        return _slots [_selectedPartsIndex].Parts;
    }

    public void ShowPanel(PartsBase parts)
    {
        _selectedPartsIndex = 0;
        SetPartsInfo(parts);
    }

    public void DeInit()
    {
        _partsPanel.SetActive(false);
        _screwPanel.SetActive(false);
    }

    public void AddPartsInfo(PartsBase parts)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].Parts == parts)
            {
                return;
            }
        }

        var slot = Instantiate(_slotsPrefab, _slotLayout).GetComponent<InfoSlotUI>();
        _slots.Add(slot);

        slot.Init(parts);

        if (_slots.Count == 1)
        {
            _selectedPartsIndex = 0;
            _slots[_selectedPartsIndex].Selected = true;

            ShowPanel(parts);
        }
    }

    public void RemovePartsInfo(PartsBase parts)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if( _slots[i].Parts == parts) 
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
                    SetPartsInfo(_slots[_selectedPartsIndex].Parts);
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
        SetPartsInfo(_slots[_selectedPartsIndex].Parts);
    }

    private void SetPartsInfo(PartsBase partsBase)
    {
        _partsPanel.SetActive(false);
        _screwPanel.SetActive(false);

        if (partsBase is Parts)
        {
            _partsPanel.SetActive(true);

            Parts parts = partsBase as Parts;

            _nameLabel.text = parts.name;

            _stat1Label.text = $"Stat1\n{parts.Stat.Stat1}";
            _stat2Label.text = $"Stat2\n{parts.Stat.Stat2}";
            _stat3Label.text = $"Stat3\n{parts.Stat.Stat3}";
            _stat4Label.text = $"Stat4\n{parts.Stat.Stat4}";

            _barImage.fillAmount = parts.Quality / 100.0f;
        }
        else
        {
            _screwPanel.SetActive(true);
        }
    }
}
