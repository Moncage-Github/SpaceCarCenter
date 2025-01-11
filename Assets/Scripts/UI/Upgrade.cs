using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrade : MonoBehaviour
{

    public void NextVehicleButton()
    {
        Debug.Log("Next");
    }

    public void PrevVehicleButton()
    {
        Debug.Log("Prev");

    }

    public void StartGameButton()
    {
        SceneManager.LoadScene("cho 1");
    }
}
