using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public GameObject heartPrefab; // Heart prefab to instantiate
    public Transform heartContainer; // Parent container for hearts

    private List<GameObject> hearts = new List<GameObject>(); // List to hold heart GameObjects
    public int maxHealth = 5; // Maximum health points

    void Start()
    {
        InitializeHealth();
    }

    // Initialize health by creating heart GameObjects
    void InitializeHealth()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heart.transform.localPosition = new Vector3(i * 50, 0, 0); // Adjust spacing as needed
            hearts.Add(heart);
        }
    }

    // Method to remove a heart
    public void RemoveHeart()
    {
        if (hearts.Count > 0)
        {
            GameObject heartToRemove = hearts[hearts.Count - 1];
            hearts.RemoveAt(hearts.Count - 1);
            Destroy(heartToRemove);
        }
    }
}
