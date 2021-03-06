﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.InputModule;

using TMPro;

public class SpatialSceneGestureManager : MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public SpatialSceneMenuManager SpatialSceneMenuManagerObject;
    IEnumerator OnHold()
    {
        yield return new WaitForSeconds(1f);
        SpatialSceneMenuManagerObject.ActivateMenu();
        yield return null;
    }

    IEnumerator ButtonDownToLoadNewScene()
    {
        yield return new WaitForSeconds(1f);

        //이벤트 해제
        InputManager.Instance.PopFallbackInputHandler();

        SceneManager.LoadScene(1, LoadSceneMode.Single);
        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
        //이벤트를 받기위한 인스턴스
    }

    // IHoldHandler는 Completed, Canceled, Started 세가지에 대한 구현 메소드가 필요하다.
    public void OnHoldStarted(HoldEventData eventData)
    {
        if (!SpatialSceneMenuManager.menuSelecting)
        {
            StartCoroutine(OnHold());
        }
    }
    public void OnHoldCompleted(HoldEventData eventData)
    {
        
    }

    public void OnHoldCanceled(HoldEventData eventData)
    {

    }
    // endof IHoldHandler Event Handler


    // IInputClickHandler 는 OnInputClicked에 대한 메소드 구현이 필요하다.
    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("tap");
        if(SpatialSceneMenuManager.menuSelecting)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, float.MaxValue))
            {
                if (hit.collider.gameObject.name == "ProceedButton")
                {
                    Debug.Log("Menu selected");
                    SpatialSceneMenuManagerObject.GetComponent<SpatialSceneMenuManager>().DeActivateMenu();
                    StartCoroutine(ButtonDownToLoadNewScene());
                    SpatialSceneMenuManager.menuSelecting = false;
                }
                 else
                {
                    Debug.Log("Menu not selected");
                    SpatialSceneMenuManagerObject.GetComponent<SpatialSceneMenuManager>().DeActivateWithoutSelectingMenu();
                    SpatialSceneMenuManager.menuSelecting = false;
                }
            }
            else
            {
                Debug.Log("Menu not selected");
                SpatialSceneMenuManagerObject.GetComponent<SpatialSceneMenuManager>().DeActivateWithoutSelectingMenu();
                SpatialSceneMenuManager.menuSelecting = false;
            }
        }
    }
    // Endof InputClickerHandler Event Handler

}
