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


        _currentTarget = GetCloseTarget();

        if (GetDistance(End.transform.position, _currentTarget.transform.position) > Threshold)
        {
            JointRobotArm current = Root;
            while (current != null)
            {
                float slope = CalculateSlope(current, _currentTarget);
                current.Rotate(slope * _rate);
                current = current.Next();
            }            
        }
    }

    float GetDistance(Vector3 point1, Vector3 point2)
    {
        return Vector3.Distance(point1, point2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO:: 수집 아이템의 태그 지정
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
