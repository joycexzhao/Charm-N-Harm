using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Include TextMeshPro namespaces

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance; // Singleton instance
    private TextMeshProUGUI enemyCountText; // Reference to the TextMeshProUGUI component
    public int enemyCount;

    public int EnemyCount // Public property to access the enemy count
    {
        get { return enemyCount; }
        set
        {
            enemyCount = value;
            UpdateEnemyCountText(); // Update the UI whenever the count changes
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevents the EnemyManager from being destroyed on scene load
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensures there is only one instance of the EnemyManager
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name != "SpawnRoom")
        {
            // Attempt to find the enemy counter UI element in the new scene
            enemyCountText = GameObject.FindWithTag("EnemyCounterTextTag").GetComponent<TextMeshProUGUI>();
            RefreshEnemyCount(); // Recalculate and update the enemy count for the new scene
        }
    }

    public void RefreshEnemyCount()
    {
        // Recalculate enemy count when scene loads or when explicitly called
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void UpdateEnemyCountText()
    {
        // Ensure the text component is found before attempting to update it
        if (enemyCountText == null)
        {
            Debug.LogWarning("EnemyCountText not found in the scene. Make sure it is tagged correctly.");
            return;
        }
        enemyCountText.text = $"Enemies Left: {EnemyCount}";
        Debug.Log(EnemyCount);
    }

    public void EnemyKilled()
    {
        EnemyCount--; // Decrement and update UI through the property
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }
}
