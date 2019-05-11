using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinator : MonoBehaviour
{
    public GameObject RayCameraPrefab;
    public GameObject LightPrefab;
    public GameObject DebugBoxPrefab;
    public Transform DebugCanvasTransform;

    public TMPro.TextMeshProUGUI DebugText;

    private Queue<InferenceResult> workQueue = null;

    public static Coordinator Instance = null;

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

        int count = 0;
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
                int id = box.id;
                float lightIntensity = GetLightIntensity(box.intensity);
                Color colorTemperature = GetColorTemperature(box.color);
                Vector3 position = hit.point;
                
                GameObject lightObject = CreateLight(position, id, lightIntensity, colorTemperature);

                count++;
            }

            // debug
            Vector3 camPos = rayCamera.transform.position;
            Debug.LogFormat("Camera: ({0}, {1}, {2}), Ray: ({3}, {4}, {5})", camPos.x, camPos.y, camPos.z, ray.direction.x, ray.direction.y, ray.direction.z);
        }

        DebugText.text = string.Format("Box count: {0}, hitted ray: {1}", data.DetectionBoxes.Length, count);

        Destroy(rayCameraObject);
    }

    Vector3 GetBoxCenter(Vector2 v1, Vector2 v2)
    {
        float yOffset = 720f;
        Vector2 v = (v1 + v2) / 2f;
        Vector3 center = new Vector3(v.x, yOffset - v.y, 0f);
        

        return center;
    }

    GameObject CreateLight(Vector3 position, int id, float intensity, Color color)
    {
        if(LightPrefab == null)
        {
            Debug.Log("please assign to light prefab.");
            return null;
        }

        GameObject lightObject = Instantiate(LightPrefab, position, Quaternion.identity, this.gameObject.transform);
        Light light = lightObject.GetComponent<Light>();
        light.type = LightType.Point;
        light.intensity = intensity * 50f;
        light.color = color;

        GameObject debugBox = Instantiate(DebugBoxPrefab);
        debugBox.transform.SetParent(DebugCanvasTransform);
        debugBox.transform.position = lightObject.transform.position;

        DebugBoxManager debugBoxManager = debugBox.GetComponent<DebugBoxManager>() as DebugBoxManager;

        if(debugBoxManager == null)
        {
            Debug.Log("light - LightingManager: missing componet");
            Destroy(lightObject);
            return null;
        }

        debugBoxManager.SetParams(id, intensity, color);

        return lightObject;
    }
    
    float GetLightIntensity(float intensity)
    {
        float lightIntensity = intensity * 50f;

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
