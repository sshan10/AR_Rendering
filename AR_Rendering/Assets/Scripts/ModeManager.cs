using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ModeManager : MonoBehaviour
{
    public GameObject MenuObject;
    public float menuGeneratingDistance = 1f;

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
        menuAnimator = MenuObject.GetComponent<Animator>();
    }

    //Menu UI Managing
    public void ActivateMenu()
    {
        if(modeSelecting == false)
        {
            MenuObject.transform.position = cameraTransform.forward * menuGeneratingDistance;
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
        MenuObject.transform.rotation = initialTransform.rotation;
        MenuObject.transform.position = initialTransform.position;

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

}
