using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // === BUTTON MANAGER SCRIPT === \\
    // This script manages button interactions in the game.
    // It handles button clicks and triggers corresponding actions.
    // Attach this script to a ButtonManager GameObject in the scene.
    // ================================= \\

    // --- Retry Button --- \\
    public void RetryButton()
    {
        // Reload the current scene to restart the game
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        Time.timeScale = 1f; // Resume time in case it was paused
    }
}
