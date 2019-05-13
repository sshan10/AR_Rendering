using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.SpatialMapping;

public class PlaySceneInitialSet : MonoBehaviour
{
    public SpatialMappingObserver onSceneMappingObserver;
    public SpatialMappingManager onSceneSpatialManager;
    public ObjectSurfaceObserver onSceneGeommetryDivider;

    // Start is called before the first frame update
    void Start()
    {
        onSceneMappingObserver = GameObject.FindGameObjectWithTag("SpatialMappingPrefab").GetComponent<SpatialMappingObserver>();
        onSceneSpatialManager = GameObject.FindGameObjectWithTag("SpatialMappingPrefab").GetComponent<SpatialMappingManager>();
        onSceneGeommetryDivider = GameObject.FindGameObjectWithTag("SpatialMappingPrefab").GetComponent<ObjectSurfaceObserver>();

        onSceneMappingObserver.enabled = false;
        onSceneSpatialManager.DrawVisualMeshes = false;
        // turned private bool variable of SpatialMappingManager.cs "autoStartObserver"to public.
        onSceneSpatialManager.autoStartObserver = false;
        onSceneGeommetryDivider.enabled = false;
    }
}
