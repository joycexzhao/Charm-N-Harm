using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if player touches door, we check for enemies before entering a new room
        Debug.Log($"Collision detected with {collision.gameObject.name}");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player has collided with the door. Checking for remaining enemies...");
            if (EnemyManager.Instance.EnemyCount == 0) // Check if all enemies are defeated
            {
                Debug.Log("All enemies defeated. Preparing to change rooms...");
                Invoke("NewRoom", 0.5f);
            }
            else
            {
                Debug.Log("There are still enemies remaining!");
            }
        }
    }


    private void NewRoom()
    {
        string[] rooms = new string[3] { "Room1", "Room2", "Room3" };
        int current = 0;
        string currentScene = SceneManager.GetActiveScene().name;
        for (int i = 0; i < rooms.Length; i++)
        {
            if (currentScene == rooms[i])
            {
                current = i;
                break; // Once you find the current room, break out of the loop
            }
        }

        // This condition checks if you are at the last room already
        // and prevents an IndexOutOfRangeException by not attempting to access an index outside the array bounds
        if (current + 1 < rooms.Length)
        {
            SceneManager.LoadScene(rooms[current + 1]);
        }
        else
        {
            // Here you can handle what happens if there's no next room. For debugging:
            Debug.Log("No next room to load or attempted to access an index outside the bounds of the array.");
        }
    }

}
