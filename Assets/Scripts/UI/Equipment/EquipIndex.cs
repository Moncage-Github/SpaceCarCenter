using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipIndex : MonoBehaviour
{
    [SerializeField] private EquipIndexNumber _equipNumber;
    public EquipIndexNumber EquipNumber {  get { return _equipNumber; } }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(RawImage rawImage, GameObject TempObject, int ExquipmentId)
    {
        rawImage.texture = TempObject.GetComponent<Image>().sprite.texture;
    }
}
