using UnityEngine;

public class LightInfo
{
    public Vector3 position;
    public Vector3 rotation;
    public Light light;
    public BoxCollider collider;

    public LightInfo()
    {
        position = Vector3.zero;
        rotation = Vector3.zero;
        light = null;
        collider = null;
    }
}
