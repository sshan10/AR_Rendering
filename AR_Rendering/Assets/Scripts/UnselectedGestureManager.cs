using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

using TMPro;

public class UnselectedGestureManager : MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public ScreenCapture captureObject;
    public ModeManager modeManagerObject;        

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
        //이벤트를 받기위한 인스턴스
    }

    // IHoldHandler는 Completed, Canceled, Started 세가지에 대한 구현 메소드가 필요하다.
    public void OnHoldStarted(HoldEventData eventData)
    {
        Debug.Log("Tap or Hold");
    }
    public void OnHoldCompleted(HoldEventData eventData)
    {
        modeManagerObject.ActivateMenu();
    }

    public void OnHoldCanceled(HoldEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    // endof IHoldHandler Event Handler


    // IInputClickHandler 는 OnInputClicked에 대한 메소드 구현이 필요하다.
    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("tapped");
        if (ScreenCapture.capturing == false)
        {
            captureObject.Capture();
        }
        //탭 했을때 캡쳐가 진행중이지 않다면 캡쳐 메소드를 호출.
    }
    // Endof InputClickerHandler Event Handler

}
