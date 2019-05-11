using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ModeManager : MonoBehaviour
{
    public Animator MenuAnimator;
    public GameObject MenuObject;
    public float menuGeneratingDistance;

    Transform initialTransform;
    Transform cameraTransform;

    public static bool MenuActivating;

    // Start is called before the first frame update
    void Start()
    {
        MenuActivating = false;
        cameraTransform = Camera.main.transform;
        initialTransform = transform;
    }

    //Menu UI Managing
    public void ActivateMenu()
    {
        MenuObject.transform.position = cameraTransform.forward * menuGeneratingDistance;
        transform.rotation = Quaternion.LookRotation(-cameraTransform.forward, cameraTransform.transform.up);

        MenuAnimator.SetTrigger("HoldGestureWithoutSelected");

        MenuActivating = false;
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
        MenuAnimator.SetTrigger("MenuTapped");
        StartCoroutine("WaitForAnimAndinitializeMenu");
    }
    //UI end

}
