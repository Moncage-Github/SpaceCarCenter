using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _timeText;

    private void OnEnable()
    {
        InitPanel();
    }

    public void InitPanel()
    {
        var collectionResult = GameManager.Instance.BeforeCollectionResult;

        SetInventoryData(collectionResult);

        SetTime(collectionResult);
    }

    private void SetInventoryData(CollectionResult? result)
    {
        string message = "";
        if (result.HasValue)
        {
            if (result.Value.InventoryInfo.Count == 0)
            {
                _resultText.text = "No items obtained";
                return;
            }

            foreach (var item in result.Value.InventoryInfo)
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

    private void SetTime(CollectionResult? result)
    {
        if (result.HasValue)
        {
             _timeText.text = Util.Stopwatch.FormatTimeToMinutesAndSeconds((int)result.Value.GameTime);

        }
    }
}
