using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private int _itemCode;
    [SerializeField] private string _itemName;

    public int ItemCode { get => _itemCode; }
}
