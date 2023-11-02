using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour
{
    public OneGrabTranslateTransformer grabAnimation;

    private Animator trialAnimation;
    private float minVal;
    private float maxVal;

    float val;

    // Start is called before the first frame update
    void Start()
    {
        trialAnimation = gameObject.GetComponent<Animator>();
        minVal = grabAnimation.Constraints.MinZ.Value;
        maxVal = grabAnimation.Constraints.MaxZ.Value;

        Debug.Log(minVal);
    }

    public void OnGrabAnimation()
    {

        val = (grabAnimation.gameObject.transform.position.z - minVal) / (maxVal - minVal);

        trialAnimation.SetFloat("AnimationValue", val);
    }
}
