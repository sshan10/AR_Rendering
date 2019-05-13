using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSceneMenuManager : MonoBehaviour
{
    public static LightSceneMenuManager Instance = null;

    public GameObject menuObject;

    private float menuGeneratingDistance = 1f;

    private Camera mixedRealityCamera;
    private Transform initialTransform;
    private Animator menuAnimator;
    public static bool menuSelecting = false;

    void Awake()
    {
        Instance = this;
    }

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
        }
    }

    //called when menu Tapped
    public void DeActivateMenu()
    {
        if (menuSelecting)
        {
            menuAnimator.SetTrigger("MenuTapped");
        }
    }
    //UI end
}
