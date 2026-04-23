using UnityEngine;  
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentState = GameState.Playing;
    }

    void Update()
    {
       
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        currentState = GameState.Paused;
        Debug.Log("Game Paused");
         
    } 

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        Debug.Log("Resumed");
    }

public void GameOver()
    {
        if (currentState == GameState.GameOver) return;

        currentState = GameState.GameOver;
        Time.timeScale = 0f; 
        Debug.Log("GAME OVER");
    }

}