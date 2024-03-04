using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;

    public void ToggleDeathScreen()
    {
        deathScreen.SetActive(!deathScreen.activeSelf);

        // Toggle the timer's activity based on the deathScreen's active state
        TimeManager.instance.ToggleTimer(!deathScreen.activeSelf);
    }
}
