using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
