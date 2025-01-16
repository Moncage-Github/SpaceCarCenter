using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEquipment : BaseEquipment
{
    [SerializeField] private float _coolTime;
    [SerializeField] private int _defenseCount;
    private SpriteRenderer _spriteRenderer;
    private float _timer;
    private bool _reloading = false;

    private void Start()
    {
        _timer = _coolTime;
        Vehicle.Barrier = _defenseCount;
        Vehicle.IsTakeDamage += Defense;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_timer > 0 && _reloading == true)
        {
            _timer -= Time.deltaTime;
        }
        else if(_timer <= 0)
        {
            Barrier();

            if(Vehicle.Barrier != _defenseCount)
            {
                _reloading = true;
                _timer = _coolTime;
            }
            else
            {
                _reloading = false;
                _timer = _coolTime;
            }
            
        }
    }

    private void Barrier()
    {
        Vehicle.Barrier++;
        //이미지 활성화
        _spriteRenderer.enabled = true;
    }
    private void Defense()
    {
        _reloading = true;
        //이미지 비활성화
        if(Vehicle.Barrier == 1)
        {
            _spriteRenderer.enabled = false;
        }
        

    }
}
