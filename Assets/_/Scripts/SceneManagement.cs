using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
    // if you would like to load by name uncomment the line below
    // public string level;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Or you can load by name
        // SceneManager.LoadScene(level);
    }
}
