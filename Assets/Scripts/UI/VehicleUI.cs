using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUI : MonoBehaviour
{
    [SerializeField] Image _fuelImage;
    [SerializeField] Image _hpImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFuelBar(float amount)
    {
        _fuelImage.fillAmount = amount;
    }

    public void ChangeHpBar(float amount)
    {
        _hpImage.fillAmount = amount;
    }
}
