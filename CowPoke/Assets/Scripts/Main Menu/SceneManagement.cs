using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    public int sceneToLoad;
    
    // Load the specified scene
    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    // Quit the application
    public void QuitGame()
    {
        Application.Quit();

        // Turns off Unity Editor Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
