using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour // This class was written completely by Daniel
{
    public string newGame;
    public string scoreboard;

    public GameObject main;
    public GameObject settings;
    public GameObject credits;

    public void NewGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(newGame);
    }
    public void Settings()
    {
        settings.SetActive(true);
        main.SetActive(false);
    }
    public void Scoreboard()
    {
        SceneManager.LoadScene(scoreboard);
    }
    public void Credits()
    {
        credits.SetActive(true);
        main.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Back()
    {
        settings.SetActive(false);
        credits.SetActive(false);
        main.SetActive(true);
    }
}