using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonEquipment : BaseEquipment
{
    [SerializeField] private float _reloadTime;
    private float _timer;
    private bool _reloading = false;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _damage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_reloading)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                _reloading = false;
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("작살 발사");

            Vector3 mousePos = GetMousePos();
            Shooting(mousePos);
            _reloading = true;
            _timer = _reloadTime;
        }
    }

    private void Shooting(Vector3 target)
    {
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(transform.position, target, Vehicle.transform, _damage);
    }
}
