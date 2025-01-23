using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameOverTypeText;
    [SerializeField] private ResultOreScrollRect _oreScrollRect;
    [SerializeField] private TextMeshProUGUI _timeText;

    public void InitPanel()
    {
        var collectionResult = GameManager.Instance.BeforeCollectionResult;
        
        string message = string.Empty;
        switch(collectionResult.Value.GameOverType)
        {
            case GameOverType.Dead:
                message = "���";
                break;
            case GameOverType.OutOfFuel:
                message = "����";
                break;
            case GameOverType.Survived:
                message = "����";
                break;
        }

        SetInventoryData(collectionResult);

        SetTime(collectionResult);
    }

    private void SetInventoryData(CollectionResult? result)
    {
        if (!result.HasValue) return;

        if (result.Value.InventoryInfo.Count == 0) return; 

        foreach (var item in result.Value.InventoryInfo)
        {
            _oreScrollRect.AddItem();
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
