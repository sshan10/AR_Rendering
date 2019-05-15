using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSceneMenuManager : MonoBehaviour
{
    public GameObject menuObject;

    private float menuGeneratingDistance = 1f;

    private Camera mixedRealityCamera;
    private Animator menuAnimator;
    public static bool menuSelecting = false;

    void Start()
    {
        menuSelecting = false;
        menuAnimator = menuObject.GetComponent<Animator>();

        mixedRealityCamera = Camera.main;
    }

    //Menu UI Managing
    public void ActivateMenu()
    {
        if (menuSelecting == false)
        {
            menuObject.transform.position = mixedRealityCamera.transform.position + mixedRealityCamera.transform.forward * menuGeneratingDistance;
            //Translate menuObject
            menuObject.transform.rotation = Quaternion.LookRotation(menuObject.transform.position - mixedRealityCamera.transform.position, mixedRealityCamera.transform.transform.up);
            //Rotate menuObject towards the player

            menuAnimator.SetTrigger("HoldGestureWithoutSelected");

            menuSelecting = true;
            Debug.Log("menuSelecting");
        }
    }

    //called when menu Tapped
    public void DeActivateMenu()
    {
         menuAnimator.SetTrigger("MenuTapped");
    }

    public void DeActivateWithoutSelectingMenu()
    {
         menuAnimator.SetTrigger("MenuNotTapped");
    }
    //UI end
}
