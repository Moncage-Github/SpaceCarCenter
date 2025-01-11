using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipIndex : MonoBehaviour
{
    [SerializeField] private EquipIndexNumber _equipNumber;
    public EquipIndexNumber EquipNumber {  get { return _equipNumber; } }

    [SerializeField] private int _currentEquipmentId = 0;
    public int CurrentEquipmentId {  get { return _currentEquipmentId; } set { _currentEquipmentId = value; } }
    public EquipmentUI CurrentEquipment;

    public void SetImage(RawImage rawImage, GameObject TempObject, int ExquipmentId)
    {
        rawImage.texture = TempObject.GetComponent<Image>().sprite.texture;
        _currentEquipmentId = ExquipmentId;
    }
}
