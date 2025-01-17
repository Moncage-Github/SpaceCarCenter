using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollectionResult
{
    public float GameTime;
    public IReadOnlyDictionary<int, int> InventoryInfo;
}

public class CollectionManager : MonoBehaviour
{
    private Util.Stopwatch _stopwatch = new Util.Stopwatch();

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        _stopwatch.Start();
    }

    public void GameOver()
    {
        CollectionResult collectionGameInfo = new CollectionResult
        {
            InventoryInfo = FindObjectOfType<VehicleInventory>().GetInventory(),
            GameTime = _stopwatch.Stop()
        };

        GameManager gameManager = GameManager.Instance;

        gameManager.BeforeCollectionResult = collectionGameInfo;
        gameManager.LoadLobbyScene();
    }
}
