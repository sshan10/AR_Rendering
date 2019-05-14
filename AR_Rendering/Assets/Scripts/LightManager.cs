using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject PointLightPrefab;
    public GameObject DebugBoxPrefab;
    public Transform DebugCanvasTransform;

    public static LightManager Instance = null;

    private List<LightInfo> lights = null;

    void Awake()
    {
        Instance = this;
        lights = new List<LightInfo>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void AddLight(LightInfo data)
    {
        lights.Add(data);
    }

    public LightInfo[] GetLights()
    {
        return lights.ToArray();
    }

    public GameObject CreateLight(DetectionBox box, Vector3 hitPoint, bool debug = false)
    {
        if (PointLightPrefab == null)
        {
            Debug.Log("please assign to light prefab.");
            return null;
        }

        GameObject lightObject = Instantiate(PointLightPrefab);        
        lightObject.transform.position = hitPoint;
        lightObject.transform.eulerAngles = Quaternion.identity.eulerAngles;
        lightObject.transform.SetParent(this.gameObject.transform);

        LightInfo lightInfo = new LightInfo()
        {
            position = lightObject.transform.position,
            rotation = lightObject.transform.eulerAngles,        
            light = ApplyLightProperties(lightObject, box)
        };

        AddLight(lightInfo);


        
        if(debug)
        {
            CreateDebugBox(lightInfo, box);
        }

        return lightObject;
    }

    public GameObject CreateDebugBox(LightInfo lightInfo, DetectionBox originInfo)
    {
        GameObject debugBox = Instantiate(DebugBoxPrefab);
        debugBox.transform.SetParent(DebugCanvasTransform);
        debugBox.transform.position = lightInfo.position;

        string lightType = (originInfo.id == 1 ? "Area" : (originInfo.id == 2 ? "Point" : "Spot" ));
        DebugBoxManager debugBoxManager = debugBox.GetComponent<DebugBoxManager>() as DebugBoxManager;
        debugBoxManager.SetParams(lightType, lightInfo.light.intensity, lightInfo.light.color);

        return debugBox;
    }

    private Light ApplyLightProperties(GameObject lightObject, DetectionBox box)
    {
        Light light = lightObject.GetComponent<Light>();

        light.type = GetLightType(box.id);
        light.intensity = GetLightIntensity(box.intensity);
        light.color = GetColorTemperature(box.color);

        return light;
    }

    LightType GetLightType(int id)
    {
        LightType type = LightType.Point;

        switch(id)
        {
            case 1:
                type = LightType.Point;
                break;

            case 2:
                type = LightType.Point;
                break;

            case 3:
                type = LightType.Spot;
                break;
        }

        return type;
    }

    private static float MAX_INTENSITY = 8f;
    float GetLightIntensity(float intensity)
    {
        float lightIntensity = intensity * MAX_INTENSITY;

        // add logic

        return lightIntensity;
    }

    Color GetColorTemperature(Color color)
    {
        Color colorTemperature = color;

        // add logic.

        return colorTemperature;
    }

    void GetRange(LightType type, out float min, out float max)
    {
        min = 0f;
        max = 100f;
    }
}
