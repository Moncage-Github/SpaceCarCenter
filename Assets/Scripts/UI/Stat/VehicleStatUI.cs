using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleStatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentGoldText;
    [SerializeField] private int _currentGold;
    // Start is called before the first frame update
    void Start()
    {
        _currentGoldText.text = _currentGold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
