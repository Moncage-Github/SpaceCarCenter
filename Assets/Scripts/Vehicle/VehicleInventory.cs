using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInventory : MonoBehaviour
{
    // 임시로 SerializableDictionary로 생성 디버그용(나중에 바꿀수도)
    // [아이템 코드, 개수]
    [SerializeField] private SerializableDictionary<int, int> _invetory;

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
}
