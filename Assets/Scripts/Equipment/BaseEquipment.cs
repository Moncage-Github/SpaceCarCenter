using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEquipment : MonoBehaviour
{
    [SerializeField] protected Vehicle Vehicle;

    

    public void SetVehivle(Vehicle vehicle)
    {
        Vehicle = vehicle;
    }

    public Vector3 GetMousePos()
    {
        // ���콺 ȭ�� ��ǥ ��������
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Z���� ī�޶���� �Ÿ��� ���� (��: 0)
        mouseScreenPosition.z = 10f;  // ī�޶�κ����� �Ÿ� (������ ����)

        // ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }
}
