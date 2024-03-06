using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject winScreen;

    public void ToggleDeathScreen()
    {
        deathScreen.SetActive(!deathScreen.activeSelf);

        // Toggle the timer's activity based on the deathScreen's active state
        TimeManager.instance.ToggleTimer(!deathScreen.activeSelf);
    }

    public void ToggleWinScreen()
    {
        winScreen.SetActive(!winScreen.activeSelf);

        // Toggle the timer's activity based on the winScreen's active state
        TimeManager.instance.ToggleTimer(!winScreen.activeSelf);
    }
}
