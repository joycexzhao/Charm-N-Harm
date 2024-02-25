using UnityEngine;
using TMPro; // Use this namespace if you are using TextMeshPro

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign this in the inspector
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0"); // Rounds to the nearest second
        timerText.text = minutes + ":" + seconds;
    }
}
