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
        // 마우스 화면 좌표 가져오기
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Z값을 카메라와의 거리로 설정 (예: 0)
        mouseScreenPosition.z = 10f;  // 카메라로부터의 거리 (적절히 설정)

        // 화면 좌표를 월드 좌표로 변환
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }
}
