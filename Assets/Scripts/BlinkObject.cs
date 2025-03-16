using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObject : MonoBehaviour
{
    private bool _isBlinking = false;
    public bool IsBlinking => _isBlinking;

    private Color _startColor;
    private SpriteRenderer _renderer;
    private Coroutine _coroutine;

    public void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

    }

    public void StartBlink()
    {
        if (_isBlinking) return;
        _isBlinking = true;

        _startColor = _renderer.color;
        _coroutine = StartCoroutine(BlinkCoroutine());
    }

    public void StopBlink()
    {
        if (!_isBlinking) return;
        _isBlinking = false;

        StopCoroutine(_coroutine);

        _renderer.color = _startColor;
    }

    private IEnumerator BlinkCoroutine()
    {
        float time = 0;

        while (true)
        {
            float t = Mathf.PingPong(time / 0.5f, 1f);
            Color color1 = Color.white;
            Color color2 = Color.gray;

            Color newColor = Color.Lerp(color1, color2, t);
            _renderer.color = newColor;

            time += Time.deltaTime;

            yield return null;
        }
    }
}
