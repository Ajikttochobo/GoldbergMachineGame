using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string nextLevel;
    [SerializeField] private string mainMenu;

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    private void Awake()
    {
        UIManager.isGamePlaying = false;
        Time.timeScale = 1;
    }
}
