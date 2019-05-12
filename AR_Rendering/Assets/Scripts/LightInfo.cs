using UnityEngine;

public class LightInfo
{
    public Vector3 position;
    public Vector3 rotation;
    public Light light;

    public LightInfo()
    {
        position = Vector3.zero;
        rotation = Vector3.zero;
        light = null;
    }
}
