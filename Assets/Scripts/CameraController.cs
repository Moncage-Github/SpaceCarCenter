using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private float _cameraSpeed;

    private void Start()
    {
        Vector3 startPos = _targetObject.transform.position;
        startPos.z = -10;

        transform.position = startPos;
    }

    private void FixedUpdate()
    {
        Vector3 updatePos = Vector2.Lerp(transform.position, _targetObject.transform.position, Time.fixedDeltaTime * _cameraSpeed);
        updatePos.z = -10;
        transform.position = updatePos;
    }
}
