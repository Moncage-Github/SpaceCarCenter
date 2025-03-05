using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameOverType
{
    None,
    Dead,
    OutOfFuel,
    Survived
}
public struct CollectionResult
{
    public GameOverType GameOverType;
    public float GameTime;
    public SerializableDictionary<CollectableItemType, int> InventoryInfo;
    public int KillCount;
    public float ReceivedDamage;
    public float InflictedDamage;
    public int DestroyedMeteorCount;
}

public class CollectionManager : MonoBehaviour
{
    private static CollectionManager _instance;
    public static CollectionManager Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CollectionManager>();
            }

            return _instance; 
        } 
    }

    private Util.Stopwatch _stopwatch = new Util.Stopwatch();

    public int KillCount;
    public int DestroyedMeteorCount;
    public float ReceivedDamage;
    public float InflictedDamage;

    /// <summary>
    /// 무기별 가한 데미지 체크
    /// </summary>
    /// <param name="weaponId"> 무기 Id </param>
    /// <param name="damage"> 무기 Damage </param>
    public Dictionary<int, float> EquipmentDamage = new Dictionary<int, float>();

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        _stopwatch.Start();
    }

    [VisibleEnum(typeof(GameOverType))]
    public void GameOver(int type)
    {
        GameOverType gameOverType = (GameOverType)type;
        GameOver(gameOverType);
    }

    public void GameOver(GameOverType type)
    {
        CollectionResult collectionGameInfo = new CollectionResult
        {
            GameOverType = type,
            InventoryInfo = FindObjectOfType<VehicleInventory>().GetInventory(),
            GameTime = _stopwatch.Stop(),
            KillCount = KillCount,
            DestroyedMeteorCount = DestroyedMeteorCount,
            ReceivedDamage = ReceivedDamage,
            InflictedDamage = InflictedDamage
        };

        GameManager gameManager = GameManager.Instance;

        gameManager.BeforeCollectionResult = collectionGameInfo;
        gameManager.LoadLobbyScene();
    }

    public void ShakeCamera(float shakeAmount, float shakeTime)
    {
        StartCoroutine(ShakeRoutine(shakeAmount, shakeTime));
    }

    IEnumerator ShakeRoutine(float shakeAmount, float shakeTime)
    {
        Vector3 position = Camera.main.transform.position;

        float timer = 0;
        while (timer <= shakeTime)
        {
            Vector3 newPos = Random.insideUnitCircle * shakeAmount;
            Camera.main.transform.position = newPos + position;
            timer += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = position;
    }

    /// <summary>
    /// 무기 ID와 데미지를 매개변수로 받으며 이를 CollectionManager의 딕셔너리에 저장한다.
    /// </summary>
    /// <param name="weaponId"> 데미지를 준 무기의 Id </param>
    /// <param name="damage"> 가한 데미지 량 </param>
    /// <returns> 성공 시 true, 아이템 Id를 찾지 못하면 false </returns>
    public bool AddWeaponDamage(int weaponId,  float damage)
    {
        if(EquipmentDamage.ContainsKey(weaponId))
        {
            EquipmentDamage[weaponId] += damage;
        }
        else
        {
            EquipmentDamage.Add(weaponId, damage);
        }

        Debug.Log(weaponId.ToString() + " : " + EquipmentDamage[weaponId].ToString());

        return true;
    }
}
