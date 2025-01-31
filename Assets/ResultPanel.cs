using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameOverTypeText;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private TextMeshProUGUI _destroyedMeteorCountText;
    [SerializeField] private TextMeshProUGUI _collectOreCountText;
    [SerializeField] private TextMeshProUGUI _inflictedDamageText;
    [SerializeField] private TextMeshProUGUI _receivedDamageText;

    [SerializeField] private ResultOreScrollRect _oreScrollRect;

    public void InitPanel()
    {
        var collectionResult = GameManager.Instance.BeforeCollectionResult;

        SetGameOverText(collectionResult);

        SetInventoryData(collectionResult);

        SetPlayInfos(collectionResult);
    }

    private void SetGameOverText(CollectionResult? result)
    {
        if (!result.HasValue) return;

        string message = string.Empty;
        switch (result.Value.GameOverType)
        {
            case GameOverType.Dead:
                message = "»ç¸Á";
                break;
            case GameOverType.OutOfFuel:
                message = "°ßÀÎ";
                break;
            case GameOverType.Survived:
                message = "º¹±Í";
                break;
        }

        _gameOverTypeText.text = message;
    }

    private void SetInventoryData(CollectionResult? result)
    {
        if (!result.HasValue) return;

        if (result.Value.InventoryInfo.Count == 0) return;

        var sortInventory = result.Value.InventoryInfo.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var item in sortInventory)
        {
            _oreScrollRect.AddItem(item.Key, item.Value);
        }
    }

    private void SetPlayInfos(CollectionResult? result)
    {
        if (!result.HasValue) return;
       
        var info = result.Value;

        _timeText.text = Util.Stopwatch.FormatTimeToMinutesAndSeconds((int)info.GameTime);
        _killCountText.text = 0.ToString();
        _destroyedMeteorCountText.text = info.DestroyedMeteorCount.ToString();

        int oreCount = 0;
        foreach (var count in info.InventoryInfo.Values)
        {
            oreCount += count;
        }

        _collectOreCountText.text = oreCount.ToString();

        _inflictedDamageText.text = $"{info.InflictedDamage:N0}";
        _receivedDamageText.text = $"{info.ReceivedDamage:N0}";
    }

}
