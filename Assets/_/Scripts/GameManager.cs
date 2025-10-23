
using UnityEngine;
using UnityEngine.UI; // ‘Text’ needs to reference the UnityEngine.UI library
using UnityEngine.SceneManagement; // Scenes are referenced via UnityEngine.SceneManagement 




public class GameManager : MonoBehaviour

{
    [SerializeField] private Player player; // assign in Inspector
    [SerializeField] private Enemy[] enemies; // Supports multiple enemies!

    public int coinsCounter = 0;
    public TMPro.TMP_Text coinText;
    public string level; // Add variables at the top of your scripts




    void Start()
    {


        Debug.Log("Battle Start!");


    }

    void Update()
    {
        coinText.text = coinsCounter.ToString();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Player attacks Enemy!");
            // Loop through all enemies in the array
            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.TakeHit(5); // Example: deal 5 damage to each enemy at start
                }
            }
        }
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        SceneManager.LoadScene(level);
    }



}


