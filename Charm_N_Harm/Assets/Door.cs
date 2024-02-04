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
        // if player touches door, we enter a new room
        // I have a slight delay hard coded in for now so that it's less disorientating
        // can do animations later
        if (collision.gameObject.tag == "Player")
        {
            Invoke("NewRoom", 0.5f);
        }
    }

    private void NewRoom()
    {
        // actually reloads the same room for now lol
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
