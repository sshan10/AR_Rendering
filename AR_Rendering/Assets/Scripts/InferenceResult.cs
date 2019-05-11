using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InferenceResult
{
    public readonly Vector3 HMDPosition;
    public readonly Vector3 HMDRotation;
    public readonly DetectionBox[] DetectionBoxes;

    public InferenceResult(Vector3 hmdPosition, Vector3 hmdRotation, DetectionBox[] detectionBoxes)
    {
        this.HMDPosition = hmdPosition;
        this.HMDRotation = hmdRotation;
        this.DetectionBoxes = detectionBoxes;
    }
}
