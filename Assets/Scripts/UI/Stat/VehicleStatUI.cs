using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleStatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentGoldText;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        _currentGoldText.text = GameManager.Instance.CurrentGold.ToString();
    }
}
