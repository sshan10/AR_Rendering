using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloToolkit.Unity.InputModule;

public class NavigationTest : MonoBehaviour, INavigationHandler
{
    private enum Axis
    {
        X,
        Y,
        Z
    };

    private float MovingSensitivity = .1f;
    private Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        Debug.Log("Navigation is started.");

        InputManager.Instance.ClearModalInputStack();
        InputManager.Instance.PushModalInputHandler(this.gameObject);
        eventData.Use();
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        Vector3 movingFactor = eventData.NormalizedOffset * MovingSensitivity * Time.deltaTime;
        Vector3 nextPos = GetNextPosition(movingFactor);

        //rigidbody.useGravity = false;
        this.transform.position += nextPos;

        eventData.Use();
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        Debug.Log("Navigation is completed.");

        InputManager.Instance.PopModalInputHandler();
        eventData.Use();
        
        //rigidbody.useGravity = true;
    }

    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        Debug.Log("Navigagion is canceled.");
        eventData.Use();
        
        //rigidbody.useGravity = true;
    }

    private Vector3 GetNextPosition(Vector3 movingFactor)
    {
        Axis currentAxis = GetMaxAxis(movingFactor);
        Vector3 nextPos = Vector3.zero;
        switch (currentAxis)
        {
            case Axis.X:
                nextPos = Vector3.back * movingFactor.x;
                break;

            case Axis.Y:
                nextPos = Vector3.up * movingFactor.y;
                break;

            case Axis.Z:
                nextPos = Vector3.right * movingFactor.z;
                break;
        }

        return nextPos;
    }

    private Axis GetMaxAxis(Vector3 vector)
    {
        Axis result = Axis.X;

        Vector3 v = new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        if (v.y > v.x && v.y > v.z)
        {
            result = Axis.Y;
        }
        else if (v.z > v.x && v.z > v.y)
        {
            result = Axis.Z;
        }

        return result;
    }
}
