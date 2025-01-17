using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerEquipmentSlot : MonoBehaviour
{
    [SerializeField] private GameObject _prefabEquipment;
    [SerializeField] private Transform _content;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        foreach(var equipment in GameManager.Instance.EquipmentData.EquipmentData)
        {
            Debug.Log("equipment »ý¼º");
            GameObject slot = Instantiate(_prefabEquipment);
            slot.GetComponent<EquipmentUI>().SetEquipment(equipment.Equipment.Explan, equipment.Equipment.ImageLog, equipment.Equipment.EquipmentId);
            slot.GetComponent<EquipmentUI>().IsState = equipment.State;

            slot.transform.SetParent(_content.transform);
            slot.transform.localScale = Vector3.one;
        }
    }
}
