using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultOreScrollRect : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GameObject _prefab;

    public void AddItem(CollectableItemType itemCode, int count)
    {
        var item = Instantiate(_prefab, _scrollRect.content.transform).GetComponent<ResultOreScrollItem>();



        item.SetData($"{itemCode.ToString()}", count, 1.5f * count);
    }
}
