using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarStatDebug : MonoBehaviour
{
    [SerializeField] private InputField _drift;
    [SerializeField] private InputField _accel;
    [SerializeField] private InputField _rotate;
    [SerializeField] private InputField _maxSpeed;

    [SerializeField] private GameObject _panel;
    private bool _isPanelActive = false;

    private Vehicle _carMove;

    private void Awake()
    {
        _carMove = GameObject.Find("Car").GetComponent<Vehicle>();
    }

    private void OnEnable()
    {
        InitText();
    }

    private void InitText()
    {
        //_drift.text = _carMove.DriftFacctor.ToString();
        //_accel.text = _carMove.AcclerationForce.ToString();
        //_rotate.text = _carMove.RotationForce.ToString();
       // _maxSpeed.text = _carMove.MaxSpeed.ToString();
    }

    public void ApplyStat()
    {
        if(float.TryParse(_accel.text, out float newAccelValue))
        {
           // _carMove.AcclerationForce = newAccelValue;
        }

        if (float.TryParse(_drift.text, out float newDriftValue))
        {
            newDriftValue = Mathf.Clamp(newDriftValue, 0.0f, 0.99f);
           // _carMove.DriftFacctor = newDriftValue;
        }

        if (float.TryParse(_rotate.text, out float newRotateValue))
        {
          //  _carMove.RotationForce = newRotateValue;
        }

        if (float.TryParse(_maxSpeed.text, out float newMaxSpeedValue))
        {
          //  _carMove.MaxSpeed = newMaxSpeedValue;
        }

        InitText();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            _isPanelActive = !_isPanelActive;
            _panel.SetActive(_isPanelActive);
        }
    }
}
