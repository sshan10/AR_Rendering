using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloToolkit.Unity.SpatialMapping;

public class LightingSceneInitialSet : MonoBehaviour
{
    public SpatialMappingObserver onSceneMappingObserver;
    public ObjectSurfaceObserver onSceneGeommetryDivider;

    private readonly string TAG_SPATIAL_MAPPING_PREFAB = "SpatialMappingPrefab";

    void Start()
    {
        GameObject SpatialMappingObject = GameObject.FindGameObjectWithTag(TAG_SPATIAL_MAPPING_PREFAB);
        onSceneMappingObserver = SpatialMappingObject.GetComponent<SpatialMappingObserver>();
        onSceneGeommetryDivider = SpatialMappingObject.GetComponent<ObjectSurfaceObserver>();

        onSceneMappingObserver.enabled = false;
        onSceneGeommetryDivider.enabled = false;
    }
}
