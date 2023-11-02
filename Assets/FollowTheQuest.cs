using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheQuest : MonoBehaviour
{
    [SerializeField]
    private Camera cameraToFollow;

    [SerializeField]
    float distance = 0.4f;
    
    Vector3 velocity = Vector3.zero;

    [SerializeField]
    float smoothTime = 0.6f;

    [SerializeField]
    bool maintainY = false;

    [SerializeField]
    bool lookAtRotFix = true;
    // Start is called before the first frame update
    void Start()
    {
        if (cameraToFollow == null)
        {
            cameraToFollow = Camera.main;
        }
    }

    private void Update()
    {

        Vector3 targetPosition = cameraToFollow.transform.TransformPoint(new Vector3(0, 0, distance));

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        var lookAtPos = new Vector3(cameraToFollow.transform.position.x, maintainY ? transform.position.y : cameraToFollow.transform.position.y, cameraToFollow.transform.position.z);

        if(lookAtRotFix)
            lookAtPos = lookAtPos + new Vector3(0, 0, 180);

        transform.LookAt(lookAtPos);

    }

}
