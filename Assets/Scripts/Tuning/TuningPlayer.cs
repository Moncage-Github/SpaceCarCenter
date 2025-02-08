using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TuningPlayer : PlayerBase
{
    private TuningPlayerInput _input;

    public static TuningPlayer Instance;

    private TuningParts _back;
    private bool _isItemPickUped = false;

    private float _defaultSpeed;
    private float _defaultJumpforce;

    private int _invenNum = 1;
    public int InvenNum 
    {
        get => _invenNum;  
        set
        {
            int id = value;
            if (Mathf.Clamp(id, 1, 3) != value) return;
            _invenNum = id;
            ChangeTool(id);
            //Debug.Log(InvenNum);
        }
    }
    public TuningTool CurTool { get; set; }
    private TuningTool _driver = new TuningTool(TuningTool.Type.Driver);
    private TuningTool _hammer = new TuningTool(TuningTool.Type.Hammer);
    private TuningTool _hand = new TuningTool(TuningTool.Type.Hand);

    [SerializeField] private Image _inventoryImage;
    [SerializeField] private Sprite _handImage;
    [SerializeField] private Sprite _driverImage;
    [SerializeField] private Sprite _hammerImage;

    public void Awake()
    {
        Instance = this;
        _input = new TuningPlayerInput();
        _defaultJumpforce = JumpForce;
        _defaultSpeed = Speed;

        CurTool = _hand;
    }

    void OnEnable()
    {
        _input.PlayerAction.Enable();
        _input.PlayerAction.Move.performed += OnMove;
        _input.PlayerAction.Move.canceled += OnMove;

        _input.PlayerAction.Jump.performed += OnJump;

        _input.PlayerAction.Down.performed += OnDown;
        _input.PlayerAction.Down.canceled += OnDown;

        _input.PlayerAction.LeftClick.performed += LeftClick;
        _input.PlayerAction.RightClick.performed += RightClick;

        _input.PlayerAction.Inven.performed += OnInvenKey;
    }
    void OnDisable()
    {
        _input.PlayerAction.Disable();
        _input.PlayerAction.Move.performed -= OnMove;

        _input.PlayerAction.Jump.performed -= OnJump;

        _input.PlayerAction.Down.performed -= OnDown;
        _input.PlayerAction.Down.canceled -= OnDown;

        _input.PlayerAction.LeftClick.performed -= LeftClick;
        _input.PlayerAction.RightClick.performed -= RightClick;

        _input.PlayerAction.Inven.performed -= OnInvenKey;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public bool EquipParts(TuningParts parts)
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

    public bool UnequipParts()
    {
        if (!_isItemPickUped) return false;
        _isItemPickUped = false;
        Speed = _defaultSpeed;
        JumpForce = _defaultJumpforce;

        _back = null;  

        return true;
    }

    public void ChangeTool(TuningTool.Type type)
    {
        switch (type)
        {
            case TuningTool.Type.Hand:
                CurTool = _hand;
                _inventoryImage.sprite = _handImage;
                break;
            case TuningTool.Type.Hammer:
                CurTool = _hammer;
                _inventoryImage.sprite = _hammerImage;
                break;
            case TuningTool.Type.Driver:
                CurTool = _driver;
                _inventoryImage.sprite = _driverImage;
                break;
        }
    }

    public void ChangeTool(int invenNum)
    {
        TuningTool.Type type = (TuningTool.Type)invenNum;
        ChangeTool(type);
    }

    public void OnInvenKey(InputAction.CallbackContext context)
    {
        string name =context.control.name;
        if(int.TryParse(name, out int invenNum))
        {
            InvenNum = invenNum;
        }
        else
        {
            var value = (int)context.ReadValue<float>();
            if (Mathf.Abs(value) == 120)
            {
                value = Mathf.Clamp(value / 120, -1, 1);
                InvenNum += value;
            }
        }
    }

    public void LeftClick(InputAction.CallbackContext context)
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
                DropParts();
                return;
            }
            return;
        }

        float dist = Vector2.Distance(transform.position, hit.collider.transform.position);
        if (dist > 3.0f) return;

        if (CurTool.ToolType == TuningTool.Type.Hand)
        {
            // 등에 있는 아이템 조립
            if (_isItemPickUped)
            {
                TuningSlot slot = hit.collider.GetComponent<TuningSlot>();
                if (slot == null) return;
                CompositionParts(slot);
            }
            else
            {
                // 등에 아무것도 없고 파츠 클릭
                TuningParts parts = hit.collider.GetComponent<TuningParts>();
                if (parts != null)
                {
                    //조립되어 있는 파츠 분리 후 줍기
                    if (parts.CurState == TuningParts.State.Composed)
                    {
                        DecompositionParts(parts);
                    }
                    //바닥에 있는 파츠 줍기
                    else if (parts.CurState == TuningParts.State.Dropped)
                    {
                        PickupParts(parts);
                    }
                }
            }
        }
        else if (CurTool.ToolType == TuningTool.Type.Driver)
        {
            TuningParts parts = hit.collider.GetComponent<TuningParts>();
            if (parts == null) return;

            if (parts.CurState != TuningParts.State.ScrewComposed) return;

            SrewParts(parts);
        }
        else if (CurTool.ToolType == TuningTool.Type.Hammer)
        {

        }
    }

    public void RightClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        int layerMask = LayerMask.GetMask("TuningInteraction");

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider == null) return;

        float dist = Vector2.Distance(transform.position, hit.collider.transform.position);
        if (dist > 3.0f) return;

        if(CurTool.ToolType == TuningTool.Type.Driver)
        {
            TuningParts parts = hit.collider.gameObject.GetComponent<TuningParts>();

            UnscrewParts(parts);
        }
    }

    private void PickupParts(TuningParts parts)
    {
        Debug.Log("Try PickUp Parts");
        parts.Pickup();
        EquipParts(parts);
    }
    private void DropParts()
    {
        Debug.Log("Drop Parts");
        _back.Drop();
        UnequipParts();
    }

    private void CompositionParts(TuningSlot slot)
    {
        if (!slot.IsEmpty())
        {
            Debug.Log("The parts is already installed");
            return;
        }

        bool isSuccess = slot.TryCompositionParts(_back);
        if (!isSuccess) return;

        UnequipParts();
    }

    private void DecompositionParts(TuningParts parts)
    {
        Debug.Log("Try DeComposite Parts");
        parts.DeCompositeFromSlot();
        EquipParts(parts);
    }

    private void SrewParts(TuningParts parts)
    {
        Debug.Log("Unscrewing");
        parts.UnSrcew();
    }

    private void UnscrewParts(TuningParts parts)
    {
        if (parts == null) return;
        if (parts.NeedsScrewTightening == false) return;
        if (parts.CurState == TuningParts.State.Composed)
        {
            if (_back == null || _back.Type != ETuningPartsType.Screw) return;
            Debug.Log("Try TightenScew");
            Destroy(_back.gameObject);
            UnequipParts();
            parts.TryTightenScrew();
            return;
        }
        else if (parts.CurState == TuningParts.State.ScrewComposed)
        {
            if (_back != null) return;
            Debug.Log("Try TightenScew");
            parts.TryTightenScrew();
            return;
        }
    }
}
