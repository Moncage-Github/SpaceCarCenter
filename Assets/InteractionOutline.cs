using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class InteractionOutline : MonoBehaviour
{
    [SerializeField] private Material _sharedMaterial;
    private Material _material;

    private bool _outline;
    public bool Outline
    {
        get => _outline;
        set
        {
            _outline = value;
            SetOutline(value);
        }
    }

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        // SpriteRenderer 컴포넌트를 가져옴
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_sharedMaterial != null)
        {
            // 인스펙터에 설정된 메터리얼을 SpriteRenderer에 적용
            _material = Instantiate(_sharedMaterial);
            _spriteRenderer.material = _material;
        }

    }

    public void SetOutline(bool enable)
    {
        if (_material.HasProperty("_OutlineEnabled"))
        {
            _material.SetFloat("_OutlineEnabled", enable ? 1 : 0);
        }
    }
}
