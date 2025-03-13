using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TollGate : NeutralObject
{
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnInteraction()
    {
        Debug.Log("중립 몹과 상호작용");
        //상호작용시 동작할 스크립트 _enemyState에 넣기
        _enemyState = new TollGateInteraction(this.gameObject, cost);

        base.OnInteraction();

        IsInteraction = false;      //중복 상호작용 여부

        return;
    }
}


public class TollGateInteraction : IEnemyState
{
    private int _cost;
    GameObject _gameObject;
    public TollGateInteraction(GameObject tollGateObject, int Cost)
    {
        _cost = Cost;
        _gameObject = tollGateObject;
    }

    public void Update(EnemyBase enemy)
    {
        if(_cost <= GameManager.Instance.CurrentGold)
        {
            CollectionManager.Instance.GameOver(GameOverType.Survived);
            Debug.Log("톨게이트 진입");
            GameManager.Instance.CurrentGold -= _cost;
        }
        else
        {
            _gameObject.GetComponent<NeutralObject>().ShowMessage("잔액이 부족합니다.", 2.0f);
        }
        //TODO:: 뭔가 이상
    }
}