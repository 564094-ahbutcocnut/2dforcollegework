using UnityEngine;
using UnityEngine.UI; // ‘Text’ needs to reference the UnityEngine.UI library


public class GameManager : MonoBehaviour
{
    public int coinsCounter = 0;
    public TMPro.TMP_Text coinText;

    void Update()
    {
        coinText.text = coinsCounter.ToString();
    }
}
