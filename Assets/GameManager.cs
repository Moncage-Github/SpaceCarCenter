using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
                    _instance._sceneLoader = singleton.AddComponent<SceneLoader>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }
    #endregion

    private SceneLoader _sceneLoader;

    [SerializeField] EquipmenScriptable _equiptmentScriptable;

    private GameState _beforeState;
    public GameState BeforeState { get => _beforeState; }

    private GameState _gameState;
    public GameState GameState { get => _gameState; }

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
        _sceneLoader = gameObject.AddComponent<SceneLoader>();
    }

    public void LoadCollectionScene(Action onComplete = null)
    {
        _beforeState = _gameState;
        _gameState = GameState.Collection;
        SceneManager.LoadScene("CollectionScene");
    }

    public void LoadLobbyScene(Action onComplete = null)
    {
        _beforeState = _gameState;
        _gameState = GameState.Lobby;
        SceneManager.LoadScene("LobbyScene");
    }
}
