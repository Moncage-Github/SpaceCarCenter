using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TuningPlayer : Player
{
    public static TuningPlayer Instance;

    private TuningParts _back;
    private bool _isItemPickUped = false;

    private float _defaultSpeed;
    private float _defaultJumpforce;

    override public void Awake()
    {
        base.Awake();
        Instance = this;
        _defaultJumpforce = JumpForce;
        _defaultSpeed = Speed;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public bool PickupParts(TuningParts parts)
    {
        if (_isItemPickUped) return false;

        _isItemPickUped = true;
        Speed *= 0.5f;
        JumpForce *= 0.8f;
        _back = parts;
        _back.transform.parent = transform;
        _back.transform.localPosition = Vector3.zero;

        return true;
    }

    public bool DropItem()
    {
        if (!_isItemPickUped) return false;
        _isItemPickUped = false;
        Speed = _defaultSpeed;
        JumpForce = _defaultJumpforce;

        _back = null;  

        return true;
    }

    public override void LeftClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        int layerMask = LayerMask.GetMask("TuningInteraction");

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        // 등에 파츠 있는 파츠 떨어뜨리기
        if (hit.collider == null)
        {
            if(_isItemPickUped == true)
            {
                Debug.Log("Drop Parts");
                _back.Drop();
                DropItem();
                return;
            }
            return;
        }

        InteractionObject interaction = hit.collider.gameObject.GetComponent<InteractionObject>();
        if (interaction == null) return;

        float dist = Vector2.Distance(transform.position, interaction.transform.position);
        if (dist > 3.0f) return;

        // 등에 있는 아이템 조립
        if (_isItemPickUped == true)
        {
            if (interaction is TuningSlot)
            {
                var slot = interaction as TuningSlot;
                Debug.Log("Try Composite Parts");
                if (!slot.IsEmpty())
       
                {
                    Debug.Log("The parts is already installed");
                    return;
                }

                bool result = slot.TryCompositionParts(_back);
                if (!result) return;
                DropItem();
            }
            else
            {
                Debug.Log("Drop Parts");
                _back.Drop();
                DropItem();
                return;
            }
        }
        else
        {
            // 등에 아무것도 없고 파츠 클릭
            TuningParts interactionParts = interaction as TuningParts;
            if (interactionParts != null)
            {
                //조립되어 있는 파츠 분리 후 줍기
                if (interactionParts.CurState == TuningParts.State.Composed)
                {
                    Debug.Log("Try DeComposite Parts");
                    interactionParts.DeCompositeFromSlot();
                    PickupParts(interactionParts);
                }
                //바닥에 있는 파츠 줍기
                else if (interactionParts.CurState == TuningParts.State.Dropped)
                {
                    Debug.Log("Try PickUp Parts");
                    interactionParts.Pickup();
                    PickupParts(interactionParts);
                }
                else if (interactionParts.CurState == TuningParts.State.ScrewComposed)
                {
                    Debug.Log("Unscrewing");
                    interactionParts.UnSrcew();

                }
            }
        }
        return;
    }

    public override void RightClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        int layerMask = LayerMask.GetMask("TuningInteraction");

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider == null) return;

        float dist = Vector2.Distance(transform.position, hit.collider.transform.position);
        if (dist > 3.0f) return;

        TuningParts parts = hit.collider.gameObject.GetComponent<TuningParts>();
        if (parts == null) return;
        if (parts.NeedsScrewTightening == false) return;
        if (parts.CurState == TuningParts.State.Composed)
        {
            if (_back.Type != ETuningPartsType.Screw || _back == null) return;
            Debug.Log("Try TightenScew");
            Destroy(_back.gameObject);
            DropItem();
            parts.TryTightenScrew();
            return;
        }
        else if(parts.CurState == TuningParts.State.ScrewComposed)
        {
            if (_back != null) return;
            Debug.Log("Try TightenScew");
            parts.TryTightenScrew();
            return;
        }
    }
}
