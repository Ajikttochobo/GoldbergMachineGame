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
}
