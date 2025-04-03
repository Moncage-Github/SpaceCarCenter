using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HpManager : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + _offset;
    }
}
