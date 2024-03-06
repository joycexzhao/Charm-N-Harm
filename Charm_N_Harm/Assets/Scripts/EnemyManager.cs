using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Include TextMeshPro namespaces

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance; // Singleton instance
    private TextMeshProUGUI enemyCountText; // Reference to the TextMeshProUGUI component
    public int enemyCount;

    public GameObject sound1;
    public GameObject sound2;
    public GameObject sound3;

    private static GameObject s1;
    private static GameObject s2;
    private static GameObject s3;

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
        s1 = sound1;
        s2 = sound2;
        s3 = sound3;
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
        //Play Sound
        int rand = UnityEngine.Random.Range(0, 3);
        if (rand == 0)
        {
            s1.GetComponent<AudioSource>().Play();
        }
        else if (rand == 1)
        {
            s2.GetComponent<AudioSource>().Play();
        }
        else
        {
            s3.GetComponent<AudioSource>().Play();
        }

    }

    public void KilledAll()
    {
        EnemyCount = 0;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }
}
