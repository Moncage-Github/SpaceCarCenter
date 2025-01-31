using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private ResultPanel _resultPanel;
    public Canvas Canvas;

    private void Start()
    {
        _resultPanel.InitPanel();

        if(GameManager.Instance.BeforeState == GameState.Collection)
        {
            ShowResultPanel();
        }
    }

    public void ShowResultPanel()
    {
        if(!GameManager.Instance.BeforeCollectionResult.HasValue) { return; }

        _resultPanel.gameObject.SetActive(true);
    }

}
