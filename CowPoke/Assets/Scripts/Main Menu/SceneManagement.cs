using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    public int sceneToLoad;
    public GameObject pauseMenu;
    
    // Load the specified scene
    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    
    // Quit the application
    public void QuitGame()
    {
        Application.Quit();

        // Turns off Unity Editor Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
        Time.timeScale = 1f;
    }
}
