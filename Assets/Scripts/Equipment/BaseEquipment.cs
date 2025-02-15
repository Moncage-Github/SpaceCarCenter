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

    /// <summary>
    /// target 바라보도록 만들어주는 함수. 객체가 타겟을 가리키는 각도가 angle 이하면 true, 이상이면
    /// false를 반환하며 이상일때 target을 보게 만들어준다.
    /// target : 타겟 객체의 Transform, rotationAngle : 회전이 필요한 각도, rotationSpeed : 회전 속도
    /// </summary>
    protected bool RotateTowardsTarget(Vector3 target, float rotationAngel, float rotationSpeed)
    {
        Vector2 targetDirection = (target - transform.position).normalized;
        Vector2 direction = transform.up.normalized;
        float angle = Vector2.Angle(targetDirection, direction);

        if (angle > rotationAngel)
        {
            // 목표 회전 계산 (현재 각도와 목표 각도 간의 회전)
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // 현재 회전 각도
            float currentAngle = transform.eulerAngles.z;

            // 회전 각도 차이 계산
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

            // 회전 속도 계산 (차이가 클수록 빠르게 회전)
            float t = Mathf.Clamp(rotationSpeed * angleDifference, 0.5f, 5f);

            // 부드럽게 회전 (LerpAngle로 회전 범위가 180도 이상일 때도 처리)
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

            // 회전 적용
            transform.rotation = Quaternion.Euler(0, 0, newAngle - 90);

            return false;
        }

        return true;
    }

}
