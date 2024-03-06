using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public bool gameActive = true;
    public GameObject music;
    public GameObject winMusic;
    public GameObject loseMusic;

    private void Awake()
    {
        if (LevelManager.instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            music.GetComponent<AudioSource>().Pause();
            loseMusic.GetComponent<AudioSource>().Play();
            _ui.ToggleDeathScreen();
            
        }

    }

    public void GameWin()
    {
        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            _ui.ToggleWinScreen();
            music.GetComponent<AudioSource>().Pause();
            winMusic.GetComponent<AudioSource>().Play();
        }

    }
}
