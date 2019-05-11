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

    Transform initialTransform;
    Transform cameraTransform;
    Animator menuAnimator;
    bool modeSelecting;

    // Start is called before the first frame update
    void Start()
    {
        modeSelecting = false;
        cameraTransform = Camera.main.transform;
        initialTransform = transform;
        menuAnimator = menuObject.GetComponent<Animator>();
    }

    //Menu UI Managing
    public void ActivateMenu()
    {
        if(modeSelecting == false)
        {
            menuObject.transform.position = cameraTransform.forward * menuGeneratingDistance;
            //Translate menuObject
            transform.rotation = Quaternion.LookRotation(-cameraTransform.forward, cameraTransform.transform.up);
            //Rotate menuObject towards the player

            menuAnimator.SetTrigger("HoldGestureWithoutSelected");

            modeSelecting = true;
        }
    }

    IEnumerator WaitForAnimAndinitializeMenu()
    {
        yield return new WaitForSeconds(0.55f);
        menuObject.transform.rotation = initialTransform.rotation;
        menuObject.transform.position = initialTransform.position;

        yield return null;
    }


    //called when menu Tapped
    void DeActivateMenu()
    {
        if (modeSelecting)
        {
            menuAnimator.SetTrigger("MenuTapped");
            StartCoroutine("WaitForAnimAndinitializeMenu");
        }
    }
    //UI end

    void SpatialMode()
    {
        spatialMappingObject.GetComponent<SpatialMappingObserver>().enabled = true;
        spatialMappingObject.GetComponent<SpatialMappingManager>().DrawVisualMeshes = true;
    }

    void MappingMode()
    {
        spatialMappingObject.GetComponent<SpatialMappingObserver>().enabled = false;
        spatialMappingObject.GetComponent<SpatialMappingManager>().DrawVisualMeshes = true;
    }

    void PlayMode()
    {
        spatialMappingObject.GetComponent<SpatialMappingObserver>().enabled = false;
        spatialMappingObject.GetComponent<SpatialMappingManager>().DrawVisualMeshes = false;
    }


}
