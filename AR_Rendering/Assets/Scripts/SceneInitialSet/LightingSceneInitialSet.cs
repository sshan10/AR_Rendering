﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloToolkit.Unity.SpatialMapping;

public class LightingSceneInitialSet : MonoBehaviour
{
    public SpatialMappingObserver onSceneMappingObserver;
    public ObjectSurfaceObserver onSceneGeommetryDivider;

    public GameObject informationObject;
    public GameObject MRCam;

    private readonly string TAG_SPATIAL_MAPPING_PREFAB = "SpatialMappingPrefab";
    private readonly string TAG_MAIN_CAMERA = "MainCamera";

    private float distanceFromCam = 2f;

    void Start()
    {
        GameObject SpatialMappingObject = GameObject.FindGameObjectWithTag(TAG_SPATIAL_MAPPING_PREFAB);
        onSceneMappingObserver = SpatialMappingObject.GetComponent<SpatialMappingObserver>();
        onSceneGeommetryDivider = SpatialMappingObject.GetComponent<ObjectSurfaceObserver>();

        onSceneMappingObserver.enabled = false;
        onSceneGeommetryDivider.enabled = false;

        MRCam = GameObject.FindGameObjectWithTag(TAG_MAIN_CAMERA);
        informationObject.transform.position = MRCam.transform.position + MRCam.transform.forward * distanceFromCam;
        informationObject.transform.rotation = Quaternion.LookRotation(informationObject.transform.position - MRCam.transform.position, MRCam.transform.up);
    }
}
