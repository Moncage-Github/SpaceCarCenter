using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPosition : MonoBehaviour
{
    [SerializeField] private bool _x, _y, _z;       //true면 타겟의 위치를 따라가고 false면 현재 위치를 유지
    [SerializeField] private Transform _player;     //미니맵의 중심에 위치할 오브젝트

    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null)
        {
            Debug.Log("미니맵 플레이어 못 찾음");
            return;
        }
        Debug.Log("미니맵");
        transform.position = new Vector3(
            (_x ? _player.transform.position.x : transform.position.x), 
            (_y ? _player.transform.position.y : transform.position.y),
            (_z ? _player.transform.position.z : transform.position.z));
    }
}
