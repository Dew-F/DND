using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject PanelGameMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PanelGameMenu.activeSelf)
            {
                BackToGame();
            } else
            {
                OpenGameMenu();
            }
        }
    }

    public void OpenGameMenu()
    {
        PanelGameMenu.SetActive(true);
        Menu.Paused = true;
    }

    public void BackToGame()
    {
        PanelGameMenu.SetActive(false);
        Menu.Paused = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
