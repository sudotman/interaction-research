using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyTriggerTest : MonoBehaviour, IHandGrabUseDelegate
{
    public void BeginUse()
    {
        throw new System.NotImplementedException();
    }

    public float ComputeUseStrength(float strength)
    {
        Debug.Log("Strength is " + strength);
        return strength;
    }

    public void EndUse()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
