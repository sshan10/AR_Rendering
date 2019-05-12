using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinator : MonoBehaviour
{
    public GameObject RayCameraPrefab;

    private Queue<InferenceResult> workQueue = null;

    public static Coordinator Instance = null;

    public void Awake()
    {
        Instance = this;
        workQueue = new Queue<InferenceResult>();
    }

    public void EnqueueLightData(InferenceResult data)
    {
        if(data != null)
        {
            workQueue.Enqueue(data);
        }
    }

    void FixedUpdate()
    {
        if(workQueue.Count > 0)
        {
            InferenceResult data = workQueue.Dequeue();
            CoordinateMapping(data);
        }
    }

    void CoordinateMapping(InferenceResult data)
    {
        if(RayCameraPrefab == null)
        {
            Debug.Log("Where is the ray camera prefab?");
            return;
        }

        GameObject rayCameraObject = Instantiate(RayCameraPrefab, data.HMDPosition, Quaternion.Euler(data.HMDRotation), this.transform);
        Camera rayCamera = rayCameraObject.GetComponent<Camera>() as Camera;
        rayCamera.aspect = Camera.main.aspect;
        rayCamera.nearClipPlane = Camera.main.nearClipPlane;
        rayCamera.farClipPlane = Camera.main.farClipPlane;

        if (rayCamera == null)
        {
            Debug.Log("ray camera hasn't camera component.");
            return;
        }


        foreach(DetectionBox box in data.DetectionBoxes)
        {
            // draw ray
            Vector3 boxCenter = GetBoxCenter(box.min, box.max);
            RaycastHit hit;
            Ray ray = rayCamera.ScreenPointToRay(boxCenter);

            // create light
            if(Physics.Raycast(ray, out hit))
            {
                GameObject lightObject = LightManager.Instance.CreateLight(box, hit.point, debug: true);
            }
        }
        Destroy(rayCameraObject);
    }

    Vector3 GetBoxCenter(Vector2 v1, Vector2 v2)
    {
        float yOffset = 720f;
        Vector2 v = (v1 + v2) / 2f;
        Vector3 center = new Vector3(v.x, yOffset - v.y, 0f);
        //Vector3 center = new Vector3(yOffset - v.y, v.x, 0f);
        

        return center;
    }
}
