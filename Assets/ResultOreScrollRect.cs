using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultOreScrollRect : MonoBehaviour
{
    private ScrollRect _scrollRect;
    [SerializeField] private GameObject _prefab;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public void AddItem()
    {
        var item = Instantiate(_prefab, _scrollRect.content.transform);
        //item.SetData("±¤¼®1", 10, 1);
    }
}
