using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Immediately loads the scene specified on awake
public class LoadOnAwake : MonoBehaviour
{
    [SerializeField]
    private string sceneName1;  // Name of the scene to load
    [SerializeField]
    private string sceneName2;  // Name of a different scene to load

    void Awake ()
    {
        if (DataManager.Data.LoreViewed[0])
        {
            SceneManager.LoadScene(sceneName1);
        }
        else
        {
            SceneManager.LoadScene(sceneName2);
        }
    }
}
