using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public enum InteractableType
{
    Grab, Poke, Pinch
}

public class InteractableIndividual : MonoBehaviour
{
    [SerializeField]
    public InteractableType typeOfInteractable;

    [SerializeField]
    private Transform mainInteractable;

    private Quaternion initialRot;

    private Vector3 positionZ;

    [HideInInspector]
    public bool currentlyActive;

    [HideInInspector]
    public bool didOnceForUpdate = false;

    public RowSizer rowSizer;

    private AudioSource audioWhileMove;

    private float audioGaugerDelta;
    private float audioGaugerRot;
    private float audioGaugerLastRot;

    LocalizedHaptics localizedHaptics;

    // Start is called before the first frame update
    void Start()
    {
        didOnceForUpdate = false;

        if (typeOfInteractable == InteractableType.Grab || typeOfInteractable == InteractableType.Pinch)
        {
            initialRot = mainInteractable.localRotation;
        }
        else if(typeOfInteractable == InteractableType.Poke)
        {
            positionZ = mainInteractable.position;
        }

        audioWhileMove = GetComponent<AudioSource>();

        localizedHaptics = transform.parent.GetComponent<LocalizedHaptics>();
       
    }

    public void StartMonitoring(RowSizer rowSizer)
    {
        this.rowSizer = rowSizer;
        currentlyActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool temp;
        if (rowSizer != null)
            temp = rowSizer.tutorialMode;
        else
            temp = false;

        if (currentlyActive || temp)
        {
            if (typeOfInteractable == InteractableType.Grab)
            {
                if (Mathf.Abs(initialRot.z - mainInteractable.localRotation.z) > 0.525f)
                {
                    Debug.LogError("Finished");
                    audioWhileMove.Stop();
                    if (didOnceForUpdate == false)
                    {
                        didOnceForUpdate = true;
                        audioWhileMove.Stop();
                        localizedHaptics.EnableDisableHaptics(0);
                        GetComponentInParent<RowSizer>().FinishedCurrentInteractable(gameObject);
                        //Invoke(nameof(CurrentlyActiveAbstractCall), 0.2f);
                    }
                }
                else if(Mathf.Abs(initialRot.z - mainInteractable.localRotation.z) > 0.001f)
                {
                    audioGaugerRot = mainInteractable.localRotation.z;

                    audioGaugerDelta += Time.deltaTime;

                    if (audioGaugerDelta > 0.1)
                    {
                        Debug.LogError(Mathf.Abs(audioGaugerLastRot - audioGaugerRot));
                        if (Mathf.Abs(audioGaugerLastRot - audioGaugerRot) > 0.05)
                        {
                            audioWhileMove.Play();
                            localizedHaptics.EnableDisableHaptics(1);
                            audioGaugerLastRot = mainInteractable.localRotation.z;
                            audioGaugerDelta = 0;
                        }
                        else
                        {
                            audioWhileMove.Stop();
                        }
                    }              
                }
                else
                {
                    audioWhileMove.Stop();
                    localizedHaptics.EnableDisableHaptics(0);
                }

                if (audioGaugerDelta > 1)
                {
                    audioWhileMove.Stop();
                    localizedHaptics.EnableDisableHaptics(0);
                }
            }
            else if(typeOfInteractable == InteractableType.Poke)
            {
                if(Mathf.Abs(positionZ.z - mainInteractable.position.z) > 0.004f)
                {
                    Debug.LogError("Finished");
                    if (didOnceForUpdate == false)
                    {
                        didOnceForUpdate = true;
                        GetComponentInParent<RowSizer>().FinishedCurrentInteractable(gameObject);
                    }
                }
                else
                {
                    //Debug.LogError("current loc diff: " + Mathf.Abs(positionZ.z - mainInteractable.position.z));
                }
            }
            else if (typeOfInteractable == InteractableType.Pinch)
            {
                if (Mathf.Abs(initialRot.y - mainInteractable.localRotation.y) > 0.699f)
                {
                    audioWhileMove.Stop();
                    localizedHaptics.EnableDisableHaptics(0);
                    Debug.LogError("Finished");
                    if (didOnceForUpdate == false)
                    {
                        didOnceForUpdate = true;
                        GetComponentInParent<RowSizer>().FinishedCurrentInteractable(gameObject);
                    }
                }
                else if (Mathf.Abs(initialRot.y - mainInteractable.localRotation.y) > 0.001f)
                {
                    audioGaugerRot = mainInteractable.localRotation.y;

                    audioGaugerDelta += Time.deltaTime;

                    if (audioGaugerDelta > 0.1)
                    {
                        Debug.LogError(Mathf.Abs(audioGaugerLastRot - audioGaugerRot));
                        if (Mathf.Abs(audioGaugerLastRot - audioGaugerRot) > 0.05)
                        {
                            audioWhileMove.Play();
                            audioGaugerLastRot = mainInteractable.localRotation.y;
                            localizedHaptics.EnableDisableHaptics(1);
                            audioGaugerDelta = 0;
                        }
                        else
                        {
                            localizedHaptics.EnableDisableHaptics(0);
                            audioWhileMove.Stop();
                        }
                    }
                }
                else
                {
                    localizedHaptics.EnableDisableHaptics(0);
                    audioWhileMove.Stop();
                }
            }
        }
    }

    public void PlayAudioControl(int index)
    {
        if (index==1)
        {
            audioWhileMove.Play();
        }
    }

    public void CurrentlyActiveAbstractCall()
    {
        foreach(InteractableColorVisual visual in GetComponentsInChildren<InteractableColorVisual>())
        {
            visual.ResetBackOutsideCall();
        }
    }

    public void ResetToOriginalPosition()
    {
        if (typeOfInteractable == InteractableType.Grab || typeOfInteractable == InteractableType.Pinch)
        {
            mainInteractable.localRotation = initialRot;
        }
        else if (typeOfInteractable == InteractableType.Poke)
        {
            mainInteractable.position = positionZ;
        }
    }

    public void FlashReset()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);

        GetComponentInChildren<Grabbable>().enabled = false;
    }

    public void FlashResetObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.gameObject.SetActive(true);
    }
}
