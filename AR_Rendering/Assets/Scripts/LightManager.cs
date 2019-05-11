using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject lightPrefab;
    public List<GameObject> lightList;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(this.transform.position, hit.point - this.transform.position, Color.red, 3f);
                CreateLight(hit.point);
            }
        }
    }

    public void CreateLight(Vector3 spawnPoint)
    {
        GameObject lightObject = Instantiate(lightPrefab, spawnPoint, Quaternion.identity, this.transform);
        Light light = lightObject.GetComponent<Light>();
        if(lightObject == null || light == null)
        {
            Debug.Log("No light was produced!");
            return;
        }

        lightList.Add(lightObject);

        int num = lightList.Count;
        float range = Random.Range(5f, 15f);
        Color color = Random.ColorHSV();
        float intensity = Random.Range(.1f, 3f);

        light.type = LightType.Point;
        light.range = range;
        light.color = color;
        //light.lightmappingMode : Deprecated
        light.intensity = intensity;

        Debug.LogFormat("Light[{0}] Position: {1}, Range: {2}, Color: {3}, Intensity: {4}", num, spawnPoint, range, color, intensity);
    }
}
