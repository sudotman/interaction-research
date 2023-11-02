using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetPositionEasy : MonoBehaviour
{
    [SerializeField]
    GameObject rowHolder;


    [SerializeField]
    GameObject uiElement;

    [SerializeField]
    GameObject endGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rowHolder != null)
        rowHolder.transform.localPosition = new Vector3(-0.266f, 0.709f, 0.19f);

        if (uiElement != null)
        {
            uiElement.transform.localPosition = new Vector3(0.132f, 0.01f, -0.095f);
            uiElement.transform.localRotation = Quaternion.Euler(new Vector3(341.33f, 0, 0));
        }

        if (endGameObject != null)
            endGameObject.transform.localPosition = new Vector3(0.155f, 0.859f, 0.079f);
    }
}
