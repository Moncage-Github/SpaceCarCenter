using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeutralObject : EnemyBase
{
    /// <summary>
    /// 중립 몹 움직임 여부
    /// </summary>
    public bool IsMove;

    /// <summary>
    /// 중립 몹 공격 여부
    /// </summary>
    public bool IsAttack;

    /// <summary>
    /// 중립 몹 우호관계 여부
    /// </summary>
    public bool IsFriendly;

    /// <summary>
    /// 중립 몹 상호작용 여부
    /// </summary>
    public bool IsInteraction;
    /// <summary>
    /// 중립 몹 스폰 위치
    /// </summary>
    [SerializeField] private List<Vector3> _spawnPosition;

    public GameObject TextObject;
    protected Vehicle vehicle;

    [SerializeField] protected int cost;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (_currentState == State.Move && IsMove) OnMove();
        else if (_currentState == State.Dead) OnDead();
        else if (_currentState == State.Skill) OnSkill();
        else if (_currentState == State.Attack) OnAttack();
        else if (_currentState == State.Interaction && IsInteraction) OnInteraction();
        else OnIdle();

        Excute();

        Debug.Log("중립 몬스터");
    }

    protected virtual void OnInteraction()
    {
        Debug.Log("중립 몹과 상호작용");
        //_currentState = State.Move;

        if (!IsInteraction)
            ShowMessage("이미 상호작용 했습니다.", 2.0f);

        return;
    }

    public override void Init()
    {
        base.Init();

        IsFriendly = true;

        if(_spawnPosition.Count > 0)
        {
            transform.position = _spawnPosition[UnityEngine.Random.Range(0, _spawnPosition.Count)];
        }
    }

    /// <summary>
    /// 중립 몬스터 위에 일정 시간동안 메시지를 띄운다.
    /// </summary>
    /// <param name="message">띄울 메시지</param>
    /// <param name="time">메시지를 띄울 시간, 매개변수를 넘겨주지 않으면 기본으로 float.MaxValue 동안 띄운다.</param>
    public void ShowMessage(string message, float time = 365.0f)
    {
        TextMeshPro text = TextObject.GetComponent<TextMeshPro>();
        string currentText = text.text;

        text.text = message;
        StartCoroutine(ShowMessageCount(text, currentText, time));
    }

    IEnumerator ShowMessageCount(TextMeshPro text, string currentText, float time)
    {
        yield return new WaitForSeconds(time); // time초 대기

        //text를 원래 문자로 변경. 만약 text가 비활성화 되었다면 활성화해서 변경 후 비활성화
        if(!text.gameObject.activeSelf)
        {
            text.enabled = true;
            text.text = currentText;
            text.enabled = false;
        }
        else
            text.text = currentText;
    }

    protected virtual void OnDestroy()
    {
        //TODO:: 아이템 드랍 오브젝트 풀 제작 시 적용
        //아이템 드랍
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        
        if (IsAttack && !IsFriendly)
        {
            base.OnTriggerStay2D(other);

        }
        else if(IsFriendly)
        {
            if (other.CompareTag("Player"))
            {
                TextObject.SetActive(true);

                if(Input.GetKeyUp(KeyCode.E))
                {
                    if(vehicle == null)
                        vehicle = other.GetComponent<Vehicle>();

                    _currentState = State.Interaction;
                }
                //우클릭 시 중립 해제
                else if(Input.GetMouseButtonDown(1))
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

                    if (hitCollider.gameObject == gameObject)
                    {
                        MakeNotFriendly();
                    }
                }
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        if (IsFriendly)
            TextObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        MakeNotFriendly();
        return;
    }

    protected virtual void MakeNotFriendly()
    {
        gameObject.tag = "Enemy";
        IsFriendly = false;

        //이미 범위 내에 neutralObject가 있어서 OnTriggerEnter2D가 작동되지 않는 문제 해결
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        //SetActive는 객체 비활성화, enabled는 컴포넌트 비활성화
        collider.enabled = false;
        collider.enabled = true;

        ShowMessage("!!!");
        TextObject.SetActive(true);
    }
}
