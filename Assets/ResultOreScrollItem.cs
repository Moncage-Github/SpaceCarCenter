using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultOreScrollItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private TextMeshProUGUI _weight;

    public void SetData(string name, int count, int weight)
    {
        _name.text = name;
        _count.text = count.ToString();
        _weight.text = $"{weight}kg";
    }
}
