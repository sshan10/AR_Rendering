﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using HoloToolkit.Unity.InputModule;

public class PlaySceneGestureManager: MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public PlaySceneMenuManager playSceneMenuManagerObject;
    IEnumerator OnHold()
    {
        yield return new WaitForSeconds(1f);
        playSceneMenuManagerObject.ActivateMenu();
        yield return null;
    }

    IEnumerator ButtonDownToLoadNewScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        yield return null;
    }


    void Start()
    {
        //이벤트를 받기위한 인스턴스
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
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
        if (LightSceneMenuManager.menuSelecting)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 1 << 9))
            {
                Debug.Log("Menu selected");
                playSceneMenuManagerObject.GetComponent<PlaySceneMenuManager>().DeActivateMenu();
                StartCoroutine(ButtonDownToLoadNewScene());
                SpatialSceneMenuManager.menuSelecting = false;
            }
            else
            {
                Debug.Log("Menu not selected");
                playSceneMenuManagerObject.GetComponent<PlaySceneMenuManager>().DeActivateWithoutSelectingMenu();
                SpatialSceneMenuManager.menuSelecting = false;
            }
        }
    }
    // Endof InputClickerHandler Event Handler

}