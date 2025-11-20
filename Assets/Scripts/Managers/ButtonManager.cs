using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // === BUTTON MANAGER SCRIPT === \\
    // This script manages button interactions in the game.
    // It handles button clicks and triggers corresponding actions.
    // Attach this script to a ButtonManager GameObject in the scene.
    // ================================= \\

    // === Main Menu Buttons === \\

    // --- Play Button --- \\
    public void PlayButton()
    {
        // Load the main game scene
        SceneManager.LoadScene("EnemyAI_Scene"); // Loads the game scene
    }

    // --- Quit Button --- \\
    public void QuitGame()
    {
        // Quit the application
        Application.Quit(); // Exits the game application
        Debug.Log("Game Quit!"); // Log message for quitting
    }

    // === Game Over Buttons === \\

    // --- Retry Button --- \\
    public void RetryButton()
    {
        // Reload the current scene to restart the game
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        Time.timeScale = 1f; // Resume time in case it was paused
    }
}
