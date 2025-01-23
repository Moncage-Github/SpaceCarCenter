using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class VehicleInventory : MonoBehaviour
{
    // [아이템 코드, 개수]
    [SerializeField] private SerializableDictionary<int, int> _invetory = new();

    public void AddItemToInventory(int itemCode)
    {
        if (_invetory.ContainsKey(itemCode))
        {
            _invetory[itemCode] += 1; // 키가 존재하면 값을 1 증가
        }
        else
        {
            _invetory[itemCode] = 1; // 키가 없으면 새로 추가하고 값을 1로 설정
        }
    }

    public SerializableDictionary<int, int> GetInventory()
    {

        return _invetory;
    }
}
