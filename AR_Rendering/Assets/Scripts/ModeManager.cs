using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class ModeManager : MonoBehaviour
{
    public GameObject menuObject;
    public float menuGeneratingDistance = 1f;
    public GameObject spatialMappingObject;
    public Camera mixedRealityCamera;

    Transform initialTransform;
    Animator menuAnimator;
    public static bool modeSelecting;

    // Start is called before the first frame update
    void Start()
    {
        modeSelecting = false;
        menuAnimator = menuObject.GetComponent<Animator>();
    }

    //Menu UI Managing
    public void ActivateMenu()
    {
        if(modeSelecting == false)
        {
            menuObject.transform.position = mixedRealityCamera.transform.position + mixedRealityCamera.transform.forward * menuGeneratingDistance;
            //Translate menuObject
            menuObject.transform.rotation = Quaternion.LookRotation(menuObject.transform.position - mixedRealityCamera.transform.position, mixedRealityCamera.transform.transform.up);
            //Rotate menuObject towards the player

            menuAnimator.SetTrigger("HoldGestureWithoutSelected");

            modeSelecting = true;
        }
    }

    //called when menu Tapped
    void DeActivateMenu()
    {
        if (modeSelecting)
        {
            menuAnimator.SetTrigger("MenuTapped");
        }
    }
    //UI end
}
