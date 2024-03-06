using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public bool gameActive = true;
    public GameObject music;
    public GameObject winMusic;
    public GameObject loseMusic;

    private AudioSource[] allAudioSources;

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    private void Awake()
    {
        
        if (SceneManager.GetActiveScene().name == "Room1")
        {
            music = GameObject.FindWithTag("bgm");
            music.GetComponent<AudioSource>().Play();
        }
        //music.GetComponent<AudioSource>().Play();
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
            StopAllAudio();
            //music.GetComponent<AudioSource>().Pause();
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
