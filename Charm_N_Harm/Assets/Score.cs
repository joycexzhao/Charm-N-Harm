using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Make sure to include this for TextMeshPro elements

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance
    private int score = 1000;
    private float time;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents the ScoreManager from being destroyed on scene load
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ensures there is only one instance of the ScoreManager
        }
    }

    void Update()
    {
        // Update the score based on time, but don't update any UI element here
        time += Time.deltaTime;
        if (time >= 1f) // Every second
        {
            score -= Mathf.FloorToInt(time); // Decrease score by 1 for each second elapsed
            time = 0; // Reset time counter
            // Note: The UI update call is removed from here
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        // Don't automatically update the UI here either
    }

    public void UpdateDeathScreenScore()
    {
        // This method is called to update the score on the death screen
        TextMeshProUGUI deathScreenScoreText = GameObject.FindWithTag("Score").GetComponent<TextMeshProUGUI>();
        if (deathScreenScoreText != null)
        {
            deathScreenScoreText.text = $"Score: {score}";
        }
        else
        {
            Debug.LogError("Death screen score TextMeshProUGUI component not found.");
        }
    }

    // Call this method when the player dies to add points based on the level
    public void PlayerDied()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Room2":
                AddScore(250);
                break;
            case "Room3":
                AddScore(500);
                break;
            case "Boss":
                AddScore(750);
                break;
            default:
                // Optionally handle other cases or do nothing
                break;
        }

        // Assuming you want to update the death screen score immediately upon death
        UpdateDeathScreenScore();
    }

    public void PlayerBeatBoss()
    {
        AddScore(1000); // Add score for beating the boss
        // You might want to update a victory screen score or something similar here
    }
}
