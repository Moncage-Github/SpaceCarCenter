using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private Text _resultText;

    private void OnEnable()
    {
        InitPanel();
    }

    public void InitPanel()
    {
        var invetory = GameManager.Instance.BeforeCollectionResult;
        string message = "";
        if (invetory.HasValue)
        {
            if (invetory.Value.InventoryInfo.Count == 0)
            {
                _resultText.text = "No items obtained";
                return;
            }

            foreach (var item in invetory.Value.InventoryInfo)
            {
                message += $"Code : {item.Key}, Count : {item.Value}\n";
            }
            _resultText.text = message;
        }
        else
        {
            _resultText.text = "No Result!";
        }
    }
}
