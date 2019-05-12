using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.SpatialMapping;

public class PlaySceneInitialSet : MonoBehaviour
{
    public SpatialMappingObserver onSceneMapppingObserver;
    public ObjectSurfaceObserver onSceneGeommetryDevider;

    // Start is called before the first frame update
    void Start()
    {
        onSceneMapppingObserver = GameObject.FindGameObjectWithTag("SpatialMappingPrefab").GetComponent<SpatialMappingObserver>();
        onSceneGeommetryDevider = GameObject.FindGameObjectWithTag("SpatialMappingPrefab").GetComponent<ObjectSurfaceObserver>();

        onSceneMapppingObserver.enabled = false;
        onSceneGeommetryDevider.enabled = false;
    }
}
