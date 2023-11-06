using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public enum PracticeRightNow
{
    grabC, grabCH, grabH,
    pokeC, pokeCH, pokeH,
    pinchC, pinchCH, pinchH
}

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;

    //n-1: grab practice hands
    //n: grab practice controller
    //n + 1: grab hands - controller
    //n + 2: grab controller - controller
    //n + 3: grab hands - hands

    //^the above logic works similarly for poke, pinch too

    //works with controllers
    [Range(1, 14)]
    public int grabPractice;

    // works with both controllers and hands
    [Range(1, 14)]
    public int pokePractice;

    // works with both controllers and hands
    [Range(1, 14)]
    public int pinchPractice;


    int currentIndexForDeterminingManager = 1;

    int cachedSceneIndex = 0;
   
    bool practiceDone = false;

    Scene currentSceneLoaded;

    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ISROResults\\results.txt";

    [SerializeField]
    LoadSceneMode loadSceneMode;

    private void Awake()
    {
        instance = this;
    }

    private void Initialize()
    {
        practiceDone = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;

        currentIndexForDeterminingManager = 1;

        LoadScene(currentIndexForDeterminingManager);

        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\ISROResults");
            File.Create(filePath);
        }
    }

    public void WriteInformation(string textToBeAdded)
    {
        string alreadyExistingData = File.ReadAllText(filePath);
        File.WriteAllText(filePath, alreadyExistingData + "\n" + DateTime.UtcNow + " : " + "Current Test: " + SceneManager.GetActiveScene().name + textToBeAdded);
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
        Debug.Log("Scene unloaded succesfully");

        if (currentIndexForDeterminingManager == 0)
            return;

        SceneManager.LoadSceneAsync(currentIndexForDeterminingManager, loadSceneMode);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.SetActiveScene(scene);
        currentSceneLoaded = scene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Loads a scene which corresponds to my custom numbering in the background.
    /// </summary>
    /// <param name="n"></param>
    public void LoadScene(int n)
    {
        currentIndexForDeterminingManager = n;
        cachedSceneIndex = n;

        if (currentIndexForDeterminingManager == 1)
        {
            SceneManager.LoadSceneAsync(n, loadSceneMode);
            return;
        }

        if (!practiceDone) 
        {
            cachedSceneIndex = n;
            practiceDone = true;
            if (n <= grabPractice+3)
            {
                if (n - grabPractice== 1 || n - grabPractice == 3)
                {
                    Debug.Log("Hands practice is required.");
                    UnloadCurrentScene(grabPractice-1);
                }
                else
                {
                    UnloadCurrentScene(grabPractice);
                }

                
            }
            else if(n > grabPractice+3 && n <= pokePractice + 3)
            {
                if (n - pokePractice == 1 || n - pokePractice == 3)
                {
                    Debug.Log("Hands practice is required.");
                    UnloadCurrentScene(pokePractice - 1);
                }
                else
                {
                    UnloadCurrentScene(pokePractice);
                }
            }
            else if (n > pokePractice + 3)
            {
                if (n - pinchPractice == 1 || n - pinchPractice == 3)
                {
                    Debug.Log("Hands practice is required.");
                    UnloadCurrentScene(pinchPractice - 1);
                }
                else
                {
                    UnloadCurrentScene(pinchPractice);
                }
            }
        }
        else
        {
            practiceDone = false;
            UnloadCurrentScene(n);
        }
        
    }

    public void LoadCachedScene()
    {
        LoadScene(cachedSceneIndex);
    }


    /// <summary>
    /// Include an index for the scene to be loaded after unloading current if required - leave it to 0 if no scene is to be loaded after unloading
    /// </summary>
    /// <param name="newLoadIndex">Index pertaining to the internal index system in PersistentSceneManager</param>
    public void UnloadCurrentScene(int newLoadIndex)
    {
        SceneManager.UnloadSceneAsync(currentSceneLoaded);
        currentIndexForDeterminingManager = newLoadIndex;
    }
}


//public void LoadSceneFromEnum(PracticeRightNow rightNow)
//{
//    cachedPractice = rightNow;

//    switch (rightNow)
//    {
//        case PracticeRightNow.grabC:
//            break;

//        case PracticeRightNow.grabCH: 
//            break;

//        case PracticeRightNow.grabH: 
//            break;

//        case PracticeRightNow.pokeC: 
//            break;

//        case PracticeRightNow.pokeCH:
//            break;
//        case PracticeRightNow.pokeH: 
//            break;
//        case PracticeRightNow.pinchC: 
//            break;
//        case PracticeRightNow.pinchCH: 
//            break;
//        case PracticeRightNow.pinchH: 
//            break;
//    }
//}
