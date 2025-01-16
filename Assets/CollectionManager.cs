using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollectionResult
{
    public IReadOnlyDictionary<int, int> InventoryInfo;
}


public class CollectionManager : MonoBehaviour
{
    public void GameOver()
    {
        CollectionResult collectionGameInfo = new CollectionResult
        {
            InventoryInfo = FindObjectOfType<VehicleInventory>().GetInventory()
        };

        GameManager gameManager = GameManager.Instance;

        gameManager.BeforeCollectionResult = collectionGameInfo;
        gameManager.LoadLobbyScene();
    }
}
