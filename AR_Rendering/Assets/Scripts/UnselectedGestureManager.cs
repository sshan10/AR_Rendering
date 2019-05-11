using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

using TMPro;

public class UnselectedGestureManager : MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public ScreenCapture captureObject;
        

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
    }
    public void OnHoldCanceled(HoldEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnHoldCompleted(HoldEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnHoldStarted(HoldEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("tapped");
        if (ScreenCapture.capturing == false)
        {
            captureObject.Capture();
        }
    }
}
