using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Make sure to include this for TextMeshPro elements
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance; // Singleton instance
    private TextMeshProUGUI timerText; // Reference to the TextMeshPro UI element, now private

    private float time;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents the TimeManager from being destroyed on scene load
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ensures there is only one instance of the TimeManager
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempt to find the TimerText UI element in the new scene

        if (SceneManager.GetActiveScene().name != "SpawnRoom")
        {
            timerText = GameObject.FindWithTag("TimerTextTag").GetComponent<TextMeshProUGUI>();
        }
       
    }

    void Update()
    {
        if (timerText == null) return; // Don't proceed if timerText hasn't been found

        time += Time.deltaTime; // Increment the timer based on the delta time between frames
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time % 1) * 100); // Adjusted for two digits of milliseconds

        // Update the UI text to include two digits for milliseconds
        timerText.text = string.Format("Time: {0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }


    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

    // Correctly implemented ToggleTimer method
    internal void ToggleTimer(bool v)
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(v);
        }
    }
}
