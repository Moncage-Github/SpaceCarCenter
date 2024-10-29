using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInventory : MonoBehaviour
{
    // �ӽ÷� SerializableDictionary�� ���� ����׿�(���߿� �ٲܼ���)
    // [������ �ڵ�, ����]
    [SerializeField] private SerializableDictionary<int, int> _invetory;

    public void AddItemToInventory(int itemCode)
    {
        if (_invetory.ContainsKey(itemCode))
        {
            _invetory[itemCode] += 1; // Ű�� �����ϸ� ���� 1 ����
        }
        else
        {
            _invetory[itemCode] = 1; // Ű�� ������ ���� �߰��ϰ� ���� 1�� ����
        }
    }
}