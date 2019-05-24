using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightComponentManager : MonoBehaviour
{
    Light thisLight;
    BoxCollider thisCollider;

    private void Awake()
    {
        thisLight = GetComponent<Light>();
        thisCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        thisLight.enabled = false;

        float parameter = thisLight.intensity / 2;
        thisCollider.size = new Vector3(1, 1, 1) * parameter;
    }
    private void OnTriggerEnter(Collider other)
    {
        thisLight.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        thisLight.enabled = false;
    }
}
