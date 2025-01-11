using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointRobotArm : MonoBehaviour
{
    public JointRobotArm Child;

    public JointRobotArm Next()
    {
        return Child;
    }

    public void Rotate(float angle)
    {
        transform.Rotate(0,0,angle);
    }
   
}
