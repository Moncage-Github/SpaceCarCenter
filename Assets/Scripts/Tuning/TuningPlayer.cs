using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Tuning
{
    public class TuningPlayer : PlayerBase
    {
        private TuningPlayerInput _input;

        public static TuningPlayer Instance;

        private PartsBase _back;
        private bool _isItemPickUped = false;

        private float _defaultSpeed;
        private float _defaultJumpforce;

        [Space(3.0f)]
        [Header("Inventory")]
        [SerializeField] private ToolInventory _inven;

        [SerializeField] private HammerMiniGame _miniGame;

        float _cameraX;

        public void Awake()
        {
            Instance = this;
            _input = new TuningPlayerInput();
            _defaultJumpforce = JumpForce;
            _defaultSpeed = Speed;
            //_inven.ChangeTool(TuningTool.Type.Hand);

            _cameraX = Camera.main.orthographicSize * Screen.width / Screen.height - transform.lossyScale.x / 2;
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

        public override void Move(float value)
        {
            if (!CanMove) return;
            Vector3 position = transform.localPosition;
            position.x += value * Speed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, -_cameraX, _cameraX);


            transform.localPosition = position;
        }


        public bool EquipParts(PartsBase parts)
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


        public void OnInvenKey(InputAction.CallbackContext context)
        {
            string name =context.control.name;

            if (int.TryParse(name, out int invenNum))
            {
                _inven.SlotNum = invenNum;
            }
            else
            {
                var value = (int)context.ReadValue<float>();
                if (Mathf.Abs(value) == 120)
                {
                    value = Mathf.Clamp(value / 120, -1, 1);
                    _inven.SlotNum += value;
                }
            }
        }

        /*
        public void LeftClick(InputAction.CallbackContext context)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            int layerMask = LayerMask.GetMask("TuningInteraction");

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

            // 등에 파츠 있는 파츠 떨어뜨리기
            if (hit.collider == null)
            {
                if (_isItemPickUped == true)
                {
                    DropParts();
                    return;
                }
                return;
            }

            float dist = Vector2.Distance(transform.position, hit.collider.transform.position);
            if (dist > 3.0f) return;

            // 등에 있는 아이템 조립
            if (_isItemPickUped)
            {
                PartsSlot slot = hit.collider.GetComponent<PartsSlot>();
                if (slot == null) return;
                CompositionParts(slot);
                return;
            }
            else
            {
                // 등에 아무것도 없고 파츠 클릭
                Parts parts = hit.collider.GetComponent<Parts>();
                if (parts != null)
                {
                    if (_inven.CurTool.ToolType == TuningTool.Type.Driver)
                    {
                        if (parts.CurState == Parts.State.ScrewComposed)
                        {
                            SrewParts(parts);

                            return;
                        }
                    }
                    else if (_inven.CurTool.ToolType == TuningTool.Type.Hammer)
                    {
                        if (parts.Quality < 100)
                        {
                            StartHammerGame(parts);
                            return;
                        }
                    }

                    //조립되어 있는 파츠 분리 후 줍기
                    if (parts.CurState == Parts.State.Composed)
                    {
                        DecompositionParts(parts);
                        return;
                    }
                    //바닥에 있는 파츠 줍기
                    else if (parts.CurState == Parts.State.Dropped)
                    {
                        PickupParts(parts);
                        return;
                    }
                }
            }
        }
        */

        public void LeftClick(InputAction.CallbackContext context)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            int layerMask = LayerMask.GetMask("TuningInteraction");

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

            if(hits.Length == 0)
            {
                if (!_isItemPickUped) return;
                if (_inven.CurTool != ToolType.Hand) return;
                DropParts();
                return; 
            }

            float dist = Vector2.Distance(transform.position, mousePos);
            if (dist > 4.0f) return;

            if (_isItemPickUped)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    var slot = hit.collider.GetComponent<PartsSlot>();

                    if (slot == null) continue;

                    if (CompositionParts(slot))
                    {
                        return;
                    }
                }
            }

            var toolType = _inven.CurTool;
            if(toolType == ToolType.Hand)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if(_isItemPickUped) break; 

                    // 등에 아무것도 없고 파츠 클릭
                    Parts parts = hit.collider.GetComponent<Parts>();
                    if (parts == null)
                    {
                        Screw screw = hit.collider.GetComponent<Screw>();
                        if (screw == null) continue;
                        PickupParts(screw);
                        return;
                    }

                    //조립되어 있는 파츠 분리 후 줍기
                    if (parts.CurState == Parts.State.Composed)
                    {
                        DecompositionParts(parts);
                        return;
                    }
                    //바닥에 있는 파츠 줍기
                    else if (parts.CurState == Parts.State.Dropped)
                    {
                        PickupParts(parts);
                        return;
                    }
                }
            }

            else if (_inven.CurTool == ToolType.ScrewDriver)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    Parts parts = hit.collider.GetComponent<Parts>();
                    if (parts == null) continue;
                    if (parts.CurState == Parts.State.ScrewComposed)
                    {
                        UnSrewParts(parts);

                        return;
                    }
                }
            }
            else if (_inven.CurTool == ToolType.Hammer)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    Parts parts = hit.collider.GetComponent<Parts>();
                    if (parts == null) return;

                    if (parts.Quality < 100)
                    {
                        StartHammerGame(parts);
                        return;
                    }
                }
            }
        }

        public void RightClick(InputAction.CallbackContext context)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            int layerMask = LayerMask.GetMask("TuningInteraction");

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider == null) return;

            float dist = Vector2.Distance(transform.position, mousePos);
            if (dist > 4.0f) return;

            if (_inven.CurTool == ToolType.ScrewDriver)
            {
                Parts parts = hit.collider.gameObject.GetComponent<Parts>();

                ScrewParts(parts);
            }
        }

        public void StartHammerGame(Parts parts)
        {
            _miniGame.gameObject.SetActive(true);
            _miniGame.Init(parts);
            _input.PlayerAction.LeftClick.performed -= LeftClick;
            _input.PlayerAction.LeftClick.performed += _miniGame.OnClick;
        }

        public void FinishHammerGame()
        {
            _input.PlayerAction.LeftClick.performed -= _miniGame.OnClick;
            _input.PlayerAction.LeftClick.performed += LeftClick;
        }

        private void PickupParts(PartsBase parts)
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

        private bool CompositionParts(PartsSlot slot)
        {
            if (!slot.IsEmpty())
            {
                Debug.Log("The parts is already installed");
                return false;
            }

            var parts = _back as Parts;
            if (parts == null) return false;

            bool isSuccess = slot.TryCompositionParts(parts);
            if (!isSuccess) return false;

            return UnequipParts();

        }

        private void DecompositionParts(Parts parts)
        {
            Debug.Log("Try DeComposite Parts");
            parts.DeCompositeFromSlot();
            EquipParts(parts);
        }

        private void UnSrewParts(Parts parts)
        {
            Debug.Log("Unscrewing");
            parts.UnSrcew();
        }

        private void ScrewParts(Parts parts)
        {
            if (parts == null) return;
            if (parts.NeedsScrewTightening == false) return;
            if (parts.CurState == Parts.State.Composed)
            {
                if (_back == null || _back is not Screw) return;
                Screw screw = _back as Screw;
                Debug.Log("Try TightenScew");
                UnequipParts();
                
                parts.SetScrew(screw);
                parts.TryTightenScrew();
                return;
            }
            else if (parts.CurState == Parts.State.ScrewComposed)
            {
                if (_back != null) return;
                Debug.Log("Try TightenScew");
                parts.TryTightenScrew();
                return;
            }
        }
    }
}