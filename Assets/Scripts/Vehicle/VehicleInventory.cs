using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInventory : MonoBehaviour
{
    // [������ �ڵ�, ����]
    [SerializeField] private Dictionary<int, int> _invetory = new();

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

    public IReadOnlyDictionary<int, int> GetInventory()
    {
        return _invetory;
    }
}
