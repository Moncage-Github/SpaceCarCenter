using Spine;
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

        private PartsData _back;
        private bool _isPickUp = false;

        private float _defaultSpeed;
        private float _defaultJumpforce;

        [Space(3.0f)]
        [Header("Inventory")]
        [SerializeField] private ToolInventory _inven;
        [SerializeField] private SpriteRenderer _backRenderer;

        [SerializeField] private HammerMiniGame _miniGame;

        float _cameraX;

        public void Awake()
        {
            _input = new TuningPlayerInput();
            _defaultJumpforce = JumpForce;
            _defaultSpeed = Speed;
            //_inven.ChangeTool(TuningTool.Type.Hand);

            _cameraX = Camera.main.orthographicSize * Screen.width / Screen.height - transform.lossyScale.x / 2;
            _back = null;
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

        public override void Move(float value)
        {
            if (!CanMove) return;
            Vector3 position = transform.localPosition;
            position.x += value * Speed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, -_cameraX, _cameraX);

            transform.localPosition = position;
        }


        public bool PickUpParts(PartsData data)
        {
            if (_isPickUp) return false;

            PartsPool.Instance.PickUpParts(data);

            _backRenderer.sprite = data.Sprite;

            _back = data;

            _isPickUp = true;
            Speed *= 0.5f;
            JumpForce *= 0.8f;

            return true;
        }

        public void PickUpScrew(Screw screw)
        {
            _isPickUp = true;
            _back = null;   
            _backRenderer.sprite = screw.GetSprite();

            PartsPool.Instance.PickUpScrew();

        }

        public void OnInvenKey(InputAction.CallbackContext context)
        {
            string name = context.control.name;

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
                    PartsPool.Instance.WheelInput(value);
                }
            }
        }

        public void LeftClick(InputAction.CallbackContext context)
        {
            var partsPool = PartsPool.Instance;
            var selectedParts = partsPool.SelectedObject;

            if (_isPickUp)
            {
                var slots = partsPool.GetAllOverlapedSlots();

                if (slots.Count == 0)
                {
                    if (_inven.CurTool == ToolType.Hand && selectedParts == null)
                    {
                        if (_back != null)
                        {
                            DropParts();
                        }
                        else
                        {
                            DropScrew();
                        }
                    }
                    return;
                }

                if (_back == null) return;

                foreach (var slot in slots)
                {
                    if (slot.Type == _back.Type && slot.HasParts == false)
                    {
                        if (!CheckDistance(slot.transform)) continue;

                        partsPool.EquipPartsAtSlot(slot, _back);

                        _isPickUp = false;
                        Speed = _defaultSpeed;
                        JumpForce = _defaultJumpforce;
                        _backRenderer.sprite = null;

                        return;
                    }
                }
            }

            if (_inven.CurTool == ToolType.Hand)
            {
                if (selectedParts is PartsSlot)
                {
                    var slot = selectedParts as PartsSlot;
                    if (!CheckDistance(slot.transform)) return;
                    if (slot.HasScrew) return;

                    var data = partsPool.UnEquipPartsAtSlot(slot);
                    PickUpParts(data);

                    return;
                }
                else if (selectedParts is Parts)
                {
                    var parts = selectedParts as Parts;
                    if (!CheckDistance(parts.transform)) return;

                    PickUpParts(parts.Data);
                    Destroy(parts.gameObject);
                }
                else if (selectedParts is Screw)
                {
                    var screw = selectedParts as Screw;

                    PickUpScrew(screw);
                    Destroy(screw.gameObject);
                }
            }
            else if(_inven.CurTool == ToolType.ScrewDriver)
            {
                if (selectedParts is not PartsSlot) return;
                var slot = selectedParts as PartsSlot;
                if (!CheckDistance(slot.transform)) return;

                if (!slot.HasScrew) return;
                slot.UntightenScrew();
            }
            else if(_inven.CurTool == ToolType.Hammer)
            {
                if (selectedParts is not PartsSlot) return;
                var slot = selectedParts as PartsSlot;
                if (!slot.HasParts || slot.Data.Quality == 100) return;

                StartHammerGame(slot.Data);
            }
        }

        public void RightClick(InputAction.CallbackContext context)
        {
            if (_inven.CurTool != ToolType.ScrewDriver) return;

            PartsSlot slot = PartsPool.Instance.GetOverlapedSlot();

            if (!slot.HasParts || !slot.Data.NeedScrew) return;

            if (slot.HasScrew)
            {
                if (_isPickUp == false)
                {
                    slot.TightenScrew();
                }
            }
            else
            {
                if (_isPickUp == true && _back == null)
                {
                    slot.EquipScrew();
                    _isPickUp = false;
                    _backRenderer.sprite = null;
                    PartsPool.Instance.DropScrwe();
                }
            }
        }

        public void StartHammerGame(PartsData data)
        {
            _miniGame.gameObject.SetActive(true);
            _miniGame.Init(data);
            _input.PlayerAction.LeftClick.performed -= LeftClick;
            _input.PlayerAction.LeftClick.performed += _miniGame.OnClick;
        }

        public void FinishHammerGame()
        {
            _input.PlayerAction.LeftClick.performed -= _miniGame.OnClick;
            _input.PlayerAction.LeftClick.performed += LeftClick;
            PartsPool.Instance.ResetUI();
        }
        private void DropParts()
        {       
            if (!_isPickUp) return;

            _isPickUp = false;
            Speed = _defaultSpeed;
            JumpForce = _defaultJumpforce;
            _backRenderer.sprite = null;

            PartsPool.Instance.DropParts();
            var dropParts = PartsPool.Instance.CreateParts(_back);
            dropParts.transform.position = transform.position;

            _back = null;
        }

        private void DropScrew()
        {
            if (!_isPickUp) return;

            _isPickUp = false;

            _backRenderer.sprite = null;
            PartsPool.Instance.DropScrwe();

            var screw = PartsPool.Instance.CreateScrew();
            screw.transform.position = transform.position;      
        }

        private bool CheckDistance(Transform other)
        {
            if(other == null) return false;

            float dist = Vector2.Distance(transform.position, other.position);
            if (dist > 5.0f) return false;
            return true;
        }
    }
}