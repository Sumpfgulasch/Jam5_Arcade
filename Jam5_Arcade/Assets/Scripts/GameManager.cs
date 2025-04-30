using Audio;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Assign your Game Over UI Panel/Canvas here in the Inspector

    void Start()
    {
        // Ensure game over screen is initially hidden
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
        Time.timeScale = 1; // Ensure time scale is normal at start
    }

    // Call this method when the player collides with an enemy body
    public void GameOver()
    {
        Debug.Log("Game Over!");
        AudioManager.Instance.Play2DAudio(AudioEvent.GameOver);
        Time.timeScale = 0; // Pause the game

        // Show the game over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Game Over Screen not assigned in GameManager!");
        }

        // Optional: Stop FMOD sounds, disable player input, etc.
        // FindObjectOfType<Player>()?.DisableInput(); // Example
    }


    // Call this method from the UI Button's OnClick event
    public void RestartGame()
    {
        Time.timeScale = 1; // Reset time scale before reloading
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Optional: Add a Quit function for a Quit button
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
        #endif
    }
}