using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
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
    [SerializeField] private Image _icon;

    [Space(3.0f)]
    [Header("Quality")]
    [SerializeField] private GameObject _qualiltyPanel;
    [SerializeField] private Image _barImage;

    public Text NameLabel { get => _nameLabel; }
    public Text Stat1Lable { get => _stat1Label; }
    public Text Stat2Lable { get => _stat2Label; }
    public Text Stat3Lable { get => _stat3Label; }
    public Text Stat4Lable { get => _stat4Label; }
    public Image Icon { get => _icon; }
    public Image BarImage { get => _barImage; }
    public GameObject QualiltyPanel { get => _qualiltyPanel; }

    public void SetName(string name) => _nameLabel.text = name;
    public void SetStat1(int stat) => _stat1Label.text = $"Stat1\n{stat}";
    public void SetStat2(int stat) => _stat2Label.text = $"Stat2\n{stat}";
    public void SetStat3(int stat) => _stat3Label.text = $"Stat3\n{stat}";
    public void SetStat4(int stat) => _stat4Label.text = $"Stat4\n{stat}";
    public void SetPartsImage(Sprite sprite) => _icon.sprite = sprite;
    public void SetQualilty(int qualilty) => _barImage.fillAmount = qualilty / 100.0f;
    public void Init()
    {
        _nameLabel.gameObject.SetActive(true);
        _stat1Label.gameObject.SetActive(true);
        _stat2Label.gameObject.SetActive(true);
        _stat3Label.gameObject.SetActive(true);
        _stat4Label.gameObject.SetActive(true);
        _icon.gameObject.SetActive(true);
        _barImage.gameObject.SetActive(true);
        _qualiltyPanel.gameObject.SetActive(true);
    }
}
