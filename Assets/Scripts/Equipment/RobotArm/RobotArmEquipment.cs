using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmEquipment : BaseEquipment
{
    public JointRobotArm Root;

    public JointRobotArm End;

    public List<GameObject> Target = new List<GameObject>();
    private GameObject _currentTarget;

    public float Threshold = 0.05f;

    [SerializeField] private float _rate = 0.5f;
    [SerializeField] private float _catchRange;
    [SerializeField] private float _takeSpeed;
    private bool _isCatch = false;

    float CalculateSlope(JointRobotArm joint, GameObject Target)
    {
        float deltaTheta = 0.01f;
        float distance1 = GetDistance(End.transform.position, Target.transform.position);

        joint.Rotate(deltaTheta);

        float distance2 = GetDistance(End.transform.position, Target.transform.position);

        joint.Rotate(-deltaTheta);

        return (distance1 - distance2) / deltaTheta;
    }

    // Update is called once per frame
    void Update()
    {
        if(Target.Count == 0) return;

        GameObject closeTarget = GetCloseTarget();
        if (_currentTarget != closeTarget)
        {
            _currentTarget = closeTarget;
            _isCatch = false;
        }
       

        if (GetDistance(End.transform.position, _currentTarget.transform.position) > Threshold)
        {
            JointRobotArm current = Root;
            while (current != null)
            {
                float slope = CalculateSlope(current, _currentTarget);
                current.Rotate(slope * _rate * 60 * Time.deltaTime);
                current = current.Next();
            }
            float distance = Vector3.Distance(End.transform.position, _currentTarget.transform.position);

            if (distance < _catchRange)
            {
                Debug.Log("��Ҵ�");
                _isCatch = true;
            }
            else
            {
                Debug.Log("������");
                _isCatch = false;
            }
        }
        if(_isCatch)
        {
            Rigidbody2D rb = _currentTarget.GetComponent<Rigidbody2D>();
            CollectableItem item = _currentTarget.GetComponent<CollectableItem>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ (�ʿ��ϸ� ���)

                // ������ ���� ��ġ ���� ���� ���
                Vector2 targetPosition = Vehicle.transform.position; // ������ ���� ��ǥ
                Vector2 currentTargetPosition = _currentTarget.transform.position;

                Vector2 direction = (targetPosition - currentTargetPosition).normalized;

                // Ÿ�� �̵�
                Vector2 nextPosition = Vector2.Lerp(currentTargetPosition, targetPosition, _takeSpeed * Time.deltaTime);
                rb.MovePosition(nextPosition);

                if (Vector2.Distance(targetPosition, currentTargetPosition) < _catchRange)
                {
                    Vehicle.GetComponent<VehicleInventory>().AddItemToInventory(item.ItemCode);
                    Destroy(_currentTarget);
                }
            }
            else
            {
                Debug.Log("rigidbody �� ã��");
            }
        }
    }

    float GetDistance(Vector3 point1, Vector3 point2)
    {
        return Vector3.Distance(point1, point2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO:: ���� �������� �±� ����
        if(collision.CompareTag("item"))
        {
            Debug.Log("find Item");
            Target.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("item"))
        {
            Debug.Log("find Item");
            Target.Remove(collision.gameObject);
        }
    }

    private GameObject GetCloseTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in Target)
        {
            float distance = Vector3.Distance(Vehicle.transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}
