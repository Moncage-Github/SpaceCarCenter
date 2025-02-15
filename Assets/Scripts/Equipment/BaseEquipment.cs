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

    /// <summary>
    /// target �ٶ󺸵��� ������ִ� �Լ�. ��ü�� Ÿ���� ����Ű�� ������ angle ���ϸ� true, �̻��̸�
    /// false�� ��ȯ�ϸ� �̻��϶� target�� ���� ������ش�.
    /// target : Ÿ�� ��ü�� Transform, rotationAngle : ȸ���� �ʿ��� ����, rotationSpeed : ȸ�� �ӵ�
    /// </summary>
    protected bool RotateTowardsTarget(Vector3 target, float rotationAngel, float rotationSpeed)
    {
        Vector2 targetDirection = (target - transform.position).normalized;
        Vector2 direction = transform.up.normalized;
        float angle = Vector2.Angle(targetDirection, direction);

        if (angle > rotationAngel)
        {
            // ��ǥ ȸ�� ��� (���� ������ ��ǥ ���� ���� ȸ��)
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // ���� ȸ�� ����
            float currentAngle = transform.eulerAngles.z;

            // ȸ�� ���� ���� ���
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

            // ȸ�� �ӵ� ��� (���̰� Ŭ���� ������ ȸ��)
            float t = Mathf.Clamp(rotationSpeed * angleDifference, 0.5f, 5f);

            // �ε巴�� ȸ�� (LerpAngle�� ȸ�� ������ 180�� �̻��� ���� ó��)
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

            // ȸ�� ����
            transform.rotation = Quaternion.Euler(0, 0, newAngle - 90);

            return false;
        }

        return true;
    }

}
