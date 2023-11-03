using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeypadMappings : MonoBehaviour
{
    PersistentSceneManager persistentSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        persistentSceneManager = PersistentSceneManager.instance;

        if (persistentSceneManager == null)
        {
            SceneManager.LoadScene(0); 
        }
    }

    public void LoadFromPersistent(int n)
    {
        persistentSceneManager.LoadScene(n);
    }

    public void LoadCachedScene()
    {
        persistentSceneManager.LoadCachedScene();
    }
}
