using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public List<string> scenes = new List<string>();

    // Use this for initialization
    void Start ()
    {
    }

    void Update ()
    {
        KeyCode sceneNum = KeyCode.Alpha0;
        for(int i = 0; i< scenes.Count; i++)
        {
            if (Input.GetKeyDown(sceneNum+i))
            {
               // Initiate.Fade(scenes[i], Color.white, 2.0f);
                Initiate.FadeToLoading(scenes[i], Color.white, 2.0f);
                //LoadingSceneManager.LoadScene(scenes[i]);
            }
        }

    }
}


