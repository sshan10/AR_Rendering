using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.InputModule;

using TMPro;

public class SpatialSceneGestureManager : MonoBehaviour, IInputClickHandler, IHoldHandler
{
    public SpatialSceneMenuManager SpatialSceneMenuManagerObject;
    public PhysicMaterial shadowMaterial;

    IEnumerator OnHold()
    {
        yield return new WaitForSeconds(1f);
        SpatialSceneMenuManagerObject.ActivateMenu();
        yield return null;
    }

    IEnumerator ButtonDownToLoadNewScene()
    {
        yield return new WaitForSeconds(1f);

        GameObject spatialMappingObject = GameObject.FindWithTag("SpatialMappingPrefab");
        if(spatialMappingObject != null)
        {
            Transform[] geometries = spatialMappingObject.GetComponentsInChildren<Transform>();

            foreach(Transform geometry in geometries)
            {
                geometry.gameObject.tag = "PartialGeometry";

                MeshCollider collider = geometry.gameObject.GetComponent<MeshCollider>();
                if(collider != null)
                {
                    collider.material = shadowMaterial;
                }
            }
        }

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
            if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)),out hit, 1<<9))
            {
                Debug.Log("Menu selected");
                SpatialSceneMenuManagerObject.GetComponent<SpatialSceneMenuManager>().DeActivateMenu();
                StartCoroutine(ButtonDownToLoadNewScene());
                SpatialSceneMenuManager.menuSelecting = false;
            }
        }
    }
    // Endof InputClickerHandler Event Handler

}
