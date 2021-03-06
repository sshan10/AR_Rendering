﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using HoloToolkit.Unity.InputModule;

public class LightSceneGestureManager : MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public LightSceneMenuManager lightSceneMenuManagerObject;

    public Material arGroundMaterial;
    
    void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
    }

    IEnumerator OnHold()
    {
        yield return new WaitForSeconds(0.5f);
        lightSceneMenuManagerObject.ActivateMenu();
        yield return null;
    }

    IEnumerator ButtonDownToLoadNewScene()
    {
        yield return new WaitForSeconds(1f);
        InputManager.Instance.PopFallbackInputHandler();
        SceneManager.LoadScene(2, LoadSceneMode.Single);

        GameObject spatialMappingObject = GameObject.Find("SpatialMapping");
        if(spatialMappingObject != null)
        {
            MeshRenderer[] renderers = 
                spatialMappingObject.GetComponentsInChildren<MeshRenderer>();

            foreach(MeshRenderer renderer in renderers)
            {
                renderer.material = arGroundMaterial;
            }
        }

        yield return null;
    }

    // IHoldHandler는 Completed, Canceled, Started 세가지에 대한 구현 메소드가 필요하다.
    public void OnHoldStarted(HoldEventData eventData)
    {
        StartCoroutine(OnHold());
    }
    public void OnHoldCompleted(HoldEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnHoldCanceled(HoldEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
    // endof IHoldHandler Event Handler


    // IInputClickHandler 는 OnInputClicked에 대한 메소드 구현이 필요하다.
    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("tap");
        if (LightSceneMenuManager.menuSelecting)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, float.MaxValue))
            {
                if (hit.collider.gameObject.name == "ProceedButton")
                {
                    Debug.Log("Menu selected");
                    lightSceneMenuManagerObject.DeActivateMenu();
                    StartCoroutine(ButtonDownToLoadNewScene());
                    LightSceneMenuManager.menuSelecting = false;
                }
                else
                {
                    Debug.Log("Menu not selected");
                    lightSceneMenuManagerObject.DeActivateWithoutSelectingMenu();
                    LightSceneMenuManager.menuSelecting = false;
                }
            }
            else
            {
                Debug.Log("Menu not selected");
                lightSceneMenuManagerObject.DeActivateWithoutSelectingMenu();
                LightSceneMenuManager.menuSelecting = false;
            }
        }
        else
        {
            if (!ScreenCapture.Capturing)
            {
                ScreenCapture.Capture();
            }
        }
    }
    // Endof InputClickerHandler Event Handler

}
