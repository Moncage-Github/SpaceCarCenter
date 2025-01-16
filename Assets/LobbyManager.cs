using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject _resultPanel;
    
    public void ShowResultPanel()
    {
        _resultPanel.SetActive(true);
    }

}
