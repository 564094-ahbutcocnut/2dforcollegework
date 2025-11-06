using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;

    [SerializeField] private Text cherriesText;
    [SerializeField] private AudioSource collectionSoundEffect;
    [SerializeField] private PlayerLife playerLife; // Reference to PlayerLife script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            cherries++;
            cherriesText.text = "Cherries: " + cherries;


            if (cherries >= 3)
            {
                // Call method to update lives in PlayerLife
                playerLife.UpdateLives(1); // Assuming 3 cherry gives 1 life]
                cherries = 0;
            }


        }
    }
}
