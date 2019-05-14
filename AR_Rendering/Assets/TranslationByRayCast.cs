using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationByRayCast : MonoBehaviour
{
    public Camera MRCamera;

    public bool placeButtonDown;
    // Start is called before the first frame update
    void Start()
    {
        MRCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlaySceneGestureManager.placing)
        {
            RaycastHit hit;
            if (Physics.Raycast(MRCamera.transform.position, MRCamera.transform.forward, out hit, 20f))
            {
                this.transform.position = hit.point;
                Quaternion lookMe = MRCamera.transform.localRotation;
                lookMe.x = 0;
                lookMe.y = 0;
                this.transform.rotation = lookMe;
            }
        }
    }
}
