using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct CollectionResult
{
    public float GameTime;
    public IReadOnlyDictionary<int, int> InventoryInfo;
    public int KillCount;
}

public class CollectionManager : MonoBehaviour
{
    private Util.Stopwatch _stopwatch = new Util.Stopwatch();

    public int KillCount;

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
            GameTime = _stopwatch.Stop(),
            KillCount = KillCount
        };

        GameManager gameManager = GameManager.Instance;

        gameManager.BeforeCollectionResult = collectionGameInfo;
        gameManager.LoadLobbyScene();
    }
}
