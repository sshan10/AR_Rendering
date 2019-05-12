using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloToolkit.Unity.InputModule;

public class LightSceneGestureManager : MonoBehaviour, IInputClickHandler, IHoldHandler
{
    IEnumerator OnHold()
    {
        yield return new WaitForSeconds(1f);
        LightSceneMenuManager.Instance.ActivateMenu();
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
        if (MenuManager.menuSelecting)
        {
            Ray ray;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), 50f, 10))
            {

            }
        } else
        {
            // On Capture

            if(ScreenCapture.Capturing)
            {
                return;
            }


            ScreenCapture.Capture();

        }
    }
    // Endof InputClickerHandler Event Handler

}
