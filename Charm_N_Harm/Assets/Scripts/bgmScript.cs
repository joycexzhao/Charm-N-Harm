using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bgmScript : MonoBehaviour
{
    public static bgmScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Boss")
        {
            bgmScript.instance.GetComponent<AudioSource>().Pause();
        }
    }
}
