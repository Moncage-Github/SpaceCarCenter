using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentScroll : MonoBehaviour
{
    private ScrollRect _scrollRect;

    private float _space = 50f;

    [SerializeField] private GameObject _uiPrefab;
    public List<RectTransform> UiObjects = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewUiObject()
    {
        var newUi = Instantiate(_uiPrefab, _scrollRect.content).GetComponent<RectTransform>();
        UiObjects.Add(newUi);

        float y = 0f;
        for (int i = 0; i < UiObjects.Count; i++)
        {
            UiObjects[i].anchoredPosition = new Vector2(0f, -y);
            y += UiObjects[i].sizeDelta.y + _space;
        }

        _scrollRect.content.sizeDelta = new Vector2(_scrollRect.content.sizeDelta.x, y);
    }

}
