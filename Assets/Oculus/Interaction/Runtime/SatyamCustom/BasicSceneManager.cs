using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartCurrent()
    {
        PersistentSceneManager.instance.LoadCachedScene();
    }
}
