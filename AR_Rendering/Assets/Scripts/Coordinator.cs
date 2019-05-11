using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinator : MonoBehaviour
{
    public static Coordinator Instance = null;
    public GameObject RayCameraPrefab;

    private Queue<InferenceResult> workQueue = null;

    public void Start()
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

        if(rayCamera == null)
        {
            Debug.Log("ray camera hasn't camera component.");
            return;
        }

        foreach(DetectionBox box in data.DetectionBoxes)
        {
            Vector3 boxCenter = GetBoxCenter(box.min, box.max);
            Debug.LogFormat("Center Position: {0}, {1}, {2}", boxCenter.x, boxCenter.y, boxCenter.z);


            // draw ray
            RaycastHit hit;
            Ray ray = rayCamera.ScreenPointToRay(boxCenter);
            Debug.DrawRay(rayCamera.transform.position, ray.direction, Color.blue, 9999f);


            // create light
            if(Physics.Raycast(ray, out hit))
            {
                LightType lightType = GetLightType(box.id);
                float lightIntensity = GetLightIntensity(box.intensity);
                Color colorTemperature = GetColorTemperature(box.color);
                
                CreateLight(lightType, lightIntensity, colorTemperature);
            }


            // debug
            Vector3 camPos = rayCamera.transform.position;
            Debug.LogFormat("Camera: ({0}, {1}, {2}), Ray: ({3}, {4}, {5})", camPos.x, camPos.y, camPos.z, ray.direction.x, ray.direction.y, ray.direction.z);
        }

        Destroy(rayCameraObject);
    }

    Vector3 GetBoxCenter(Vector2 v1, Vector2 v2)
    {
        float yOffset = 720f;
        Vector2 v = (v1 + v2) / 2f;
        Vector3 center = new Vector3(v.x, yOffset - v.y, 0f);
        

        return center;
    }

    GameObject CreateLight(LightType lightType, float intensity, Color color)
    {
        GameObject lightObject = null;

        // instantiate
        // set type, intensity, color temperature

        return lightObject;
    }

    LightType GetLightType(int label_id)
    {
        LightType type = LightType.Point;

        // add logic

        return type;
    }

    float GetLightIntensity(float intensity)
    {
        float lightIntensity = intensity * 1f;

        // add logic

        return lightIntensity;
    }

    Color GetColorTemperature(Color color)
    {
        Color colorTemperature = Color.white;

        // add logic.

        return colorTemperature;
    }

    void GetRange(LightType type, out float min, out float max)
    {
        min = 0f;
        max = 100f;
    }
}
