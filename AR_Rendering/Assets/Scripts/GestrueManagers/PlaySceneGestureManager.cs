using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using HoloToolkit.Unity.InputModule;

public class PlaySceneGestureManager: MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public GameObject castingObstacle;
    public PlaySceneMenuManager playSceneMenuManagerObject;
    public static bool placing;
    public GameObject interactionContentsParent;

    IEnumerator OnHold()
    {
        yield return new WaitForSeconds(0.5f);
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
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, float.MaxValue))
        {
            Debug.Log("Tap");
            if (hit.collider.gameObject.name == "PlaceButton" && !placing)
            {
                Debug.Log("Menu selected");
                playSceneMenuManagerObject.DeActivateMenu();
                castingObstacle.SetActive(true);
                placing = true;
                PlaySceneMenuManager.menuSelecting = false;
            }
            else if(placing)
            {
                placing = false;
                Transform[] transforms = interactionContentsParent.GetComponentsInChildren<Transform>();

                foreach (Transform transform in transforms)
                {
                    transform.gameObject.layer = LayerMask.NameToLayer("Default");
                }
                Debug.Log("placing Complete");
            }
            else if(hit.collider.gameObject.name == "ResetButton")
            {
                Debug.Log("Reset Menu selected");
                playSceneMenuManagerObject.DeActivateMenu();
                StartCoroutine(ButtonDownToLoadNewScene());
                PlaySceneMenuManager.menuSelecting = false;
            }
            else if (hit.collider.gameObject.name == "RestartButton")
            {
                Debug.Log("Menu not selected");
                playSceneMenuManagerObject.DeActivateMenu();
                // 초기화  do 
                PlaySceneMenuManager.menuSelecting = false;
            }
        }
        else
        {
            Debug.Log("Menu not selected");
            playSceneMenuManagerObject.DeActivateWithoutSelectingMenu();
            PlaySceneMenuManager.menuSelecting = false;
        }
    }
    // Endof InputClickerHandler Event Handler

}
