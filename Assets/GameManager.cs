using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    Lobby,
    Collection,
}


public class GameManager : MonoBehaviour
{
    #region singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).ToString());
                    _instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] EquiptmentScriptable _equiptmentScriptable;

    private GameState _gameState;

    public EquipmentsData EquipmentData;

    private CollectionResult? _beforeCollectionInfo = null;

    public CollectionResult? BeforeCollectionResult
    {
        get => _beforeCollectionInfo;
        set => _beforeCollectionInfo = value;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        EquipmentData = new EquipmentsData(_equiptmentScriptable);
    }

    public void LoadLobbyScene()
    {
        _gameState = GameState.Lobby;
        SceneManager.LoadScene("LobbyScene");
    }
}
