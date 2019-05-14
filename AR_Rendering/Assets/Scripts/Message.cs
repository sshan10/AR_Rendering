using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public Vector3 hmdPosition;
    public Vector3 hmdRotation;
    public byte[] imageRawData;

    public Message()
    {
        hmdPosition = Vector3.zero;
        hmdRotation = Vector3.zero;
        imageRawData = null;
    }

    public override string ToString()
    {
        string result = string.Empty;

        string position = Vector3ToString(hmdPosition);
        string rotation = Vector3ToString(hmdRotation);

        result = string.Format("{0}:{1}:", position, rotation);
        return result;
    }

    private string Vector3ToString(Vector3 value)
    {
        string result = string.Empty;
        result = string.Format("{0}|{1}|{2}", value.x, value.y, value.z);

        return result;
    }
}