using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // 1. Don't forget this!

public class UIManager : MonoBehaviour
{
    // 2. This must be INSIDE the class brackets
    [SerializeField] private TextMeshProUGUI buttonText;

    public void StartGame()
    {
        Time.timeScale = 1f; // Always reset time when starting/restarting
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Game");
    }
    
    public void TogglePause()
    {
            
        if (GameManager.Instance.currentState == GameState.Playing)
        {
            GameManager.Instance.PauseGame();
            buttonText.text = "Resume";
        }
        else if (GameManager.Instance.currentState == GameState.Paused)
        {
            GameManager.Instance.ResumeGame();
            buttonText.text = "Pause";
        }
    }
}