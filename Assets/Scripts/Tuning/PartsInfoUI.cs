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
    [SerializeField] private List<Image> _slots;

    int _slotsCount;

    public void ShowPanel(PartsBase parts)
    {
        _partsPanel.SetActive(false);
        _screwPanel.SetActive(false);

        if(parts is  Parts)
            ShowPanel(parts as Parts);

        else if (parts is Screw)
            ShowPanel(parts as Screw);
    }

    public void ShowPanel(Screw screw)
    {
        _screwPanel.SetActive(true);
    }

    public void ShowPanel(Parts parts)
    {
        _partsPanel.SetActive(true);

        _nameLabel.text = parts.name;


        _stat1Label.text = $"Stat1\n{parts.Stat.Stat1}";
        _stat2Label.text = $"Stat2\n{parts.Stat.Stat2}";
        _stat3Label.text = $"Stat3\n{parts.Stat.Stat3}";
        _stat4Label.text = $"Stat4\n{parts.Stat.Stat4}";

        _barImage.fillAmount = parts.Quality / 100.0f;
    }

    public void DeInit()
    {
        _partsPanel.SetActive(false);
        _screwPanel.SetActive(false);
    }

    public void AddPartsInfo(PartsBase parts)
    {
        _slotsCount++;
        _slots[_slotsCount - 1].gameObject.SetActive(true) ; 
    }

    public void RemovePartsInfo(PartsBase parts)
    {
        _slots[_slotsCount - 1].gameObject.SetActive(false);
        _slotsCount--;
    }
}
