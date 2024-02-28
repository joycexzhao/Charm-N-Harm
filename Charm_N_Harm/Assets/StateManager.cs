using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public void ChangeSceenByName(string name)
    {
        if (name != null)
        {
            SceneManager.LoadScene(name);
        }
        
    }
}
