using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    /// <summary>
    /// Restarts the level
    /// </summary>
    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1f;
    }
}
