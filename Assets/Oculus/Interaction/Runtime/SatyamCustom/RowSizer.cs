using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

[ExecuteInEditMode]
public class RowSizer : MonoBehaviour
{
    [SerializeField]
    int rowSizingX;

    [SerializeField]
    float spacingX;

    [SerializeField]
    int rowSizingY;

    [SerializeField]
    float spacingY;

    [SerializeField]
    bool allowPrefabRot;

    public GameObject interactablePrefab;

    private List<GameObject> objectsSpawned = new List<GameObject>();

    GameObject currentChosenObject;

    [HideInInspector]
    public ResearchManager researchManager;

    [SerializeField]
    public bool tutorialMode;


    // Start is called before the first frame update
    void Start()
    {
        if (tutorialMode)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<InteractableIndividual>().StartMonitoring(this);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [CallInEditor]
    void CreateList()
    {
        if (transform.childCount > 0)
        {
            //transform.DestroyChildren();

            //foreach(GameObject obj in objectsSpawned)
            //{
            //    DestroyImmediate(obj);
            //}
            Debug.LogError("Reset before proceeding - children still exist.");
            return;
        }

        if(!(rowSizingX>0 && rowSizingY > 0))
        {
            Debug.LogError("Check the row sizings - shouldn't be zero");
            return;
        }


        for(int i = 0; i < rowSizingX; i++)
        {
            GameObject tempObj = Instantiate(interactablePrefab, this.transform);

            objectsSpawned.Add(tempObj);

            tempObj.transform.SetPositionAndRotation(transform.position, allowPrefabRot ? tempObj.transform.rotation : transform.rotation);

            tempObj.transform.SetLocalPositionAndRotation(new Vector3(i * spacingX, 0, 0), allowPrefabRot ? tempObj.transform.rotation : Quaternion.identity);
            for (int j = 1; j < rowSizingY; j++)
            {
                GameObject tempObj2 = Instantiate(interactablePrefab, this.transform);
                objectsSpawned.Add(tempObj2);

                tempObj2.transform.SetPositionAndRotation(transform.position, allowPrefabRot ? tempObj.transform.rotation : transform.rotation);

                tempObj2.transform.SetLocalPositionAndRotation(new Vector3(i * spacingX, j*spacingY, 0), allowPrefabRot ? tempObj.transform.rotation : Quaternion.identity);
            }
        }

        //for (int i = 0; i < rowSizingY; i++)
        //{
        //    GameObject tempObj = Instantiate(interactablePrefab, this.transform);


        //    tempObj.transform.SetPositionAndRotation(transform.position, transform.rotation);

        //    tempObj.transform.SetLocalPositionAndRotation(new Vector3(i * spacingX, 0, 0), transform.rotation);
        //}

        
    }

    [InspectorButton("FlashOneRandom")]
    public char flash;
    public void FlashOneRandom()
    {
        if (tutorialMode)
        {
            return;
        }


        objectsSpawned.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            objectsSpawned.Add(transform.GetChild(i).gameObject);
        }

        //objectsSpawned[Random.Range(1, 4)].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<InteractableColorVisual>().UpdateVisualManual();

        currentChosenObject = objectsSpawned[Random.Range(0, objectsSpawned.Count)];
        currentChosenObject.GetComponent<InteractableIndividual>().StartMonitoring(this);
        foreach (InteractableColorVisual colorVisual in currentChosenObject.GetComponentsInChildren<InteractableColorVisual>())
        {
            Debug.LogError("Reaching yellow");
            colorVisual.FlashYellow();
        }
    }


    public void ResetAllInRowSizer()
    {
        ResetAndReplace();
        //StartCoroutine(Respawn());   
    }

    public void ResetAllWithoutDestroying()
    {
        //Reset();
        //StartCoroutine(Respawn());
        foreach (InteractableIndividual individual in GetComponentsInChildren<InteractableIndividual>())
        {
            Debug.LogError("Resetting all");
            individual.currentlyActive = false;
            individual.didOnceForUpdate = false;
            individual.ResetToOriginalPosition();
        }

        //foreach(InteractableColorVisual colorVisual in GetComponentsInChildren<InteractableColorVisual>())
        //{
        //    colorVisual.ResetBackOutsideCall();
        //}
    }

    public void FinishedCurrentInteractable(GameObject obj)
    {
        researchManager.PlaySond(1);

        foreach (InteractableColorVisual colorVisual in obj.GetComponentsInChildren<InteractableColorVisual>())
        {
            Debug.LogError("Reaching Green");
            colorVisual.FlashGreen();
        }

        researchManager.Invoke("ResetAndRestart", 1f);
        //researchManager.StartCoroutine("ResetAllQualities");
    }

    IEnumerator Respawn()
    {
        yield return new WaitForEndOfFrame();
        CreateList();
    }

    [InspectorButton("Reset")]
    public char resetAll;

    void Reset()
    {
        if (transform.childCount > 0)
        {
            transform.DestroyChildren();
        }
        //foreach (GameObject obj in objectsSpawned)
        //{
        //    if (obj == null)
        //        continue;
            
        //    if (Application.isPlaying)
        //    {
        //        objectsSpawned.Remove(obj);
        //        Destroy(obj);
        //    }
        //    else
        //    {
        //        objectsSpawned.Remove(obj);
        //        DestroyImmediate(obj);
        //    }
        //}
        objectsSpawned.Clear();
    }

    void ResetAndReplace()
    {
        if (transform.childCount == 0)
        {
            Debug.LogError("This shouldn't happen.");
            return;
        }

        List<GameObject> tempList = new List<GameObject>(); 

        foreach(GameObject obj in objectsSpawned)
        {
            GameObject currentOrig = obj;
            GameObject newReplacement = Instantiate(interactablePrefab, currentOrig.transform.position, currentOrig.transform.rotation, this.transform);

            //Debug.LogWarning(transform.childCount);
            Destroy(currentOrig);

            tempList.Add(newReplacement);
        }

        objectsSpawned.Clear();

        objectsSpawned = tempList;

        //for(int i = 0; i < transform.childCount; i++)
        //{
        //    GameObject currentOrig = transform.GetChild(i).gameObject;
        //    GameObject newReplacement = Instantiate(currentOrig, this.transform);

        //    Debug.LogWarning(transform.childCount);
        //    //Destroy(currentOrig);

        //    //objectsSpawned.Add(newReplacement);
        //}
    }
}
