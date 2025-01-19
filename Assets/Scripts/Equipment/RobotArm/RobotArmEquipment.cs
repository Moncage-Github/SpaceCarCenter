using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RobotArmEquipment : BaseEquipment
{
    public JointRobotArm Root;

    public JointRobotArm End;
    private JointRobotArm _endRigid;

    private GameObject _currentItem;

    public List<GameObject> Target = new List<GameObject>();
    private GameObject _currentTarget;

    public float Threshold = 0.05f;

    [SerializeField] private float _rate = 0.5f;
    [SerializeField] private float _catchRange;
    [SerializeField] private float _collectRange;
    [SerializeField] private float _takeSpeed;
    private List<Vector3> _defaultPos = new List<Vector3>();
    private List<bool> _defaultRotateCompleted = new List<bool>();
    private bool _isCatch = false;

    private void Start()
    {
        _endRigid = GetComponent<JointRobotArm>();
        JointRobotArm current = Root;

        while (current != null)
        {
            
            _defaultPos.Add(current.transform.localRotation.eulerAngles);
            _defaultRotateCompleted.Add(false);
            current = current.Next();
        }
    }

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
        //���ƿ��� �ڵ�
        if (Target.Count == 0)
        {
            JointRobotArm current = Root;
            int i = 0;

            while (current != null)
            {
                if (_defaultRotateCompleted[i])
                {
                    i++;
                    current = current.Next();
                    continue; // �̹� ȸ�� �Ϸ��� ��� �ǳʶ�
                }

                // ���� ȸ���� ��ǥ ȸ�� ���̸� ����
                current.transform.localRotation = Quaternion.RotateTowards(
                    current.transform.localRotation,
                    Quaternion.Euler(_defaultPos[i]),
                    _rate * Time.deltaTime
                );

                // ��ǥ ȸ���� �����ߴ��� Ȯ��
                if (Quaternion.Angle(current.transform.localRotation, Quaternion.Euler(_defaultPos[i])) < 0.1f)
                {
                    current.transform.localRotation = Quaternion.Euler(_defaultPos[i]); // ��Ȯ�� ����
                    _defaultRotateCompleted[i] = true; // ȸ�� �Ϸ� ǥ��
                }

                current = current.Next();
                i++;
            }

            return;
        }

        GameObject closeTarget = GetCloseTarget();

        if (!_isCatch && _currentTarget != closeTarget)
        {
            _currentTarget = closeTarget;
        }
       

        if (GetDistance(End.transform.position, _currentTarget.transform.position) > Threshold || _isCatch == true)
        {
            JointRobotArm current = Root;
            int i = 0;
            while (current != null)
            {
                float slope;
                if (!_isCatch)
                {
                    //�������� ��� ���� ���������� �̵�
                    slope = CalculateSlope(current, _currentTarget);
                }
                else
                {
                    //�������� ���� �Ŀ� �������� �̵�
                    slope = CalculateSlope(current, Vehicle.gameObject);
                }
                
                current.Rotate(slope * _rate * 60 * Time.deltaTime);
                current = current.Next();
                _defaultRotateCompleted[i] = false;
                i++;
            }
            float distance = Vector3.Distance(End.transform.position, _currentTarget.transform.position);
            


            if (distance < _catchRange && _isCatch == false)
            {
                Debug.Log("��Ҵ�");
                
                _currentTarget.transform.SetParent(End.transform);
                _currentTarget.transform.localPosition = new Vector3(-0.5f, 0, 0);
                //_currentTarget.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                Rigidbody2D rb = _currentTarget.GetComponent<Rigidbody2D>();
                rb.velocity = Vector3.zero;
                Destroy(rb);
                _isCatch = true;
            }
            //else
            //{
            //    Debug.Log("������");
            //    _isCatch = false;
            //}
        }
        if(_isCatch)
        {
            CollectableItem item = _currentTarget.GetComponent<CollectableItem>();
            
            //// ������ ���� ��ġ ���� ���� ���
            Vector2 targetPosition = Vehicle.transform.position; // ������ ���� ��ǥ
            //Vector2 handArmPosition = End.transform.position;
            Vector2 currentTargetPosition = _currentTarget.transform.position;

            //Vector2 direction = (targetPosition - currentTargetPosition).normalized;

            //// Ÿ�� �̵�
            //Vector2 nextPosition = Vector2.Lerp(currentTargetPosition, targetPosition, _takeSpeed * Time.deltaTime);
            //rb.MovePosition(nextPosition);

            if (Vector2.Distance(targetPosition, currentTargetPosition) < _collectRange)
            {
                Vehicle.GetComponent<VehicleInventory>().AddItemToInventory(item.ItemCode);
                Target.Remove(_currentTarget);
                Destroy(_currentTarget);
                _isCatch = false;
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
