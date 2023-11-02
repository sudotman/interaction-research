using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LastInfo
{
    int highestStreak;
}

public class ResearchManager : MonoBehaviour
{
    [SerializeField]
    private RowSizer rowSizer;

    [SerializeField]
    private TextMeshPro textForTimer;

    [SerializeField]
    private TextMeshPro textForStreak;

    private float currentTime = 0.0f;

    bool internalDoNow = false;
    bool internalCanStartResearch = false;

    int currentStreak = 0;

    [SerializeField]
    private AudioSource incorrectSource;

    [SerializeField]
    private AudioSource correctSource;

    [SerializeField]
    private GameObject endGame;

    [SerializeField]
    private GameObject[] endGameDisablers;

    int correctAttempts = 0;
    int incorrectAttempts = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        rowSizer.researchManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (internalDoNow)
        {
            textForTimer.text = "time left: " + (30 - currentTime).ToString("N1") + "s.";
            textForStreak.text = "highest streak: " + currentStreak;

            if(!( (30 - currentTime) > 0.0f))
            {
                internalDoNow = false;
                internalCanStartResearch = false;   

                for (int i = 0; i<transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                foreach(GameObject obj in endGameDisablers)
                {
                    obj.SetActive(false);
                }

                endGame.SetActive(true);

                endGame.transform.GetChild(endGame.transform.childCount-1).GetComponentInChildren<TextMeshPro>().text = "Highest Streak: " + currentStreak + "\n" + "Correctly Done: " + correctAttempts + "\n" + "Incorrectly Done: " + incorrectAttempts;

            }
        }
        else if (internalCanStartResearch)
        {
            textForTimer.text = "ready to go in " + (4 - (int)currentTime).ToString() + "s.";

            if (currentTime > 3.95)
            {
                StartResearch();
            }
        }
        else
        {
            textForTimer.text = "waiting.";
        }
    }

    void Initialize()
    {
        currentTime = 0;
        textForTimer.text = "ready to go in " + (currentTime).ToString() + "s.";
        internalCanStartResearch = true;
    }

    void StartResearch()
    {
        currentTime = 0;
        textForTimer.text = "time left: " + (30 - currentTime).ToString() + "s.";

        rowSizer.FlashOneRandom();
        internalDoNow = true;
    }

    void ContinueResearch()
    {
        textForTimer.text = "time left: " + (30 - currentTime).ToString() + "s.";

        rowSizer.FlashOneRandom();
        internalDoNow = true;
    }

    public void ResetStreak()
    {
        currentStreak = 0;
        incorrectAttempts++;
        incorrectSource.Stop();
        incorrectSource.Play();
    }

    public void ResetAndRestart()
    {
        rowSizer.ResetAllInRowSizer();
        correctAttempts++;
        currentStreak++;

        Invoke(nameof(ContinueResearch), 0.5f);
    }

    public void PlaySond(int index)
    {
        if(index==1)
            correctSource.Play();
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
