using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using System.Linq;
using Oculus.Interaction.HandGrab.Visuals;

public class HideHandGrab : MonoBehaviour
{

    [SerializeField]
    private ControllerMeshType controllerMeshType;
    [SerializeField]
    private GameObject ovrController;
    [SerializeField]
    private GameObject ovrControllerHands;
    [SerializeField]
    private HandType handType;
    [SerializeField]
    private List<GameObject> leftControllerHandMeshs;
    [SerializeField]
    private List<GameObject> rightControllerHandMeshs;

    private List<HandGrabInteractor> handGrabInteractors;

    enum ControllerMeshType
    {
        hands,
        conrtroller
    }
    enum HandType
    {
        hide,
        show
    }
    private void Awake()
    {
        if (controllerMeshType == ControllerMeshType.conrtroller)
        {
            ovrControllerHands.SetActive(false);
        }
        else
        {
            ovrController.SetActive(false);
        }

        HideHandAndControllerOnSelect();
    }

    private void HideHandAndControllerOnSelect()
    {
        handGrabInteractors = FindObjectsOfType<HandGrabInteractor>().ToList();

        List<PointableUnityEventWrapper> allEventWrappers = FindObjectsOfType<PointableUnityEventWrapper>().ToList();

        foreach (PointableUnityEventWrapper eventWrapper in allEventWrappers)
        {

            eventWrapper.WhenSelect.AddListener(delegate { OnHandControllerGrab();});
            eventWrapper.WhenUnselect.AddListener(delegate { OnHandControllerRelease(); });
        }
    }
    public void OnHandControllerGrab()
    {
        
        
        if (handType == HandType.hide)
        {
            foreach (HandGrabInteractor item in handGrabInteractors)
            {
                if (item.Hand.Handedness == Handedness.Left && item.SelectedInteractable?.HandGrabPoses.Count == 0)
                {
                    OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
                    foreach (GameObject lefthandSkin in leftControllerHandMeshs)
                    {
                        lefthandSkin.SetActive(false);
                    }
                }
                if (item.Hand.Handedness == Handedness.Right && item.SelectedInteractable?.HandGrabPoses.Count == 0)
                {
                    OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
                    foreach (GameObject righthandSkin in rightControllerHandMeshs)
                    {
                        righthandSkin.SetActive(false);
                    }
                }
            }
        }
    }
    public void OnHandControllerRelease()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

        if (handType == HandType.hide)
        {
            foreach (HandGrabInteractor item in handGrabInteractors)
            {
                if (item.Hand.Handedness == Handedness.Left)
                {
                    foreach (GameObject lefthandSkin in leftControllerHandMeshs)
                    {
                        lefthandSkin.SetActive(true);
                    }
                }
                if (item.Hand.Handedness == Handedness.Right)
                {
                    foreach (GameObject righthandSkin in rightControllerHandMeshs)
                    {
                        righthandSkin.SetActive(true);
                    }
                }
            }
        }
    }
}
