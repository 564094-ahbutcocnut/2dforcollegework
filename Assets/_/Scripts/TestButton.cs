using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public HealthManager healthManager;

    public void OnButtonClick()
    {
        healthManager.RemoveHeart();
    }
}

