using UnityEngine;
using TMPro; // Add this line to use TextMeshPro components

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    private float startTime;
    public TextMeshProUGUI timerText; // Change this line to use TextMeshProUGUI

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene loads.

            startTime = Time.time; // Initialize startTime when the game starts.
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensures only one instance of the object is active.
        }
    }

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2"); // Display seconds with two decimal places.

        timerText.text = "Timer: " + minutes + ":" + seconds;
    }
}
