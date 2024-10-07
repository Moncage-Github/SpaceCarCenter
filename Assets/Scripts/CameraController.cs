using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private Vector2 _mapSize;
    
    private Camera _camera;

    private Vector2 _minBound;
    private Vector2 _maxBound;

    private float _halfWidth;
    private float _halfHeight;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        Vector3 startPos = _targetObject.transform.position;
        startPos.z = -10;

        transform.position = startPos;

        _minBound = new Vector2(-(_mapSize.x / 2), -(_mapSize.y / 2));
        _maxBound = new Vector2((_mapSize.x / 2), (_mapSize.y / 2));
        _halfHeight = _camera.orthographicSize;
        _halfWidth = _halfHeight * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 updatePos = Vector2.Lerp(transform.position, _targetObject.transform.position, Time.fixedDeltaTime * _cameraSpeed);

        updatePos.x = Mathf.Clamp(updatePos.x, _minBound.x + _halfWidth, _maxBound.x - _halfWidth);
        updatePos.y = Mathf.Clamp(updatePos.y, _minBound.y + _halfHeight, _maxBound.y - _halfHeight);
        updatePos.z = transform.position.z;

        transform.position = updatePos;
    }
}
