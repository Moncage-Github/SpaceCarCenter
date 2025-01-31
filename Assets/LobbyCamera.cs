using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.x = _target.position.x;
        transform.position = newPos;
    }
}
