using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using System.Collections.Generic;

public class ScreenCapture : MonoBehaviour
{
    public static bool Capturing = false;

    private static PhotoCapture photoCaptureObject = null;
    private static Vector3 hmdPosition, hmdRotation;
    private static Texture2D capturedTexture = null;

    public static void Capture()
    {
        Capturing = true;

        SetHMDTransform();

        Resolution webcamResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        capturedTexture = new Texture2D(webcamResolution.width, webcamResolution.height);

        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters()
            {
                hologramOpacity = 0.0f,
                cameraResolutionWidth = webcamResolution.width,
                cameraResolutionHeight = webcamResolution.height,
                pixelFormat = CapturePixelFormat.BGRA32
            };

            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                photoCaptureObject.TakePhotoAsync(OnCapturePhotoToMemory);
            });
        });
    }

    private static void OnCapturePhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if(!result.success)
        {
            return;
        }
        
        photoCaptureFrame.UploadImageDataToTexture(capturedTexture);

        Message message = new Message()
        {
            hmdPosition = GetHMDPosition(),
            hmdRotation = GetHMDRotation(),
            imageRawData = capturedTexture.EncodeToJPG(100)
        };
    
        if (Client.Instance != null &&  Client.Instance.IsServerRunning())
        {
            Client.Instance.SendToServer(message);
        }

        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);

        Capturing = false;
    }

    private static void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private static void SetHMDTransform()
    {
        hmdPosition = Camera.main.transform.position;
        hmdRotation = Camera.main.transform.eulerAngles;
    }
    
    private static Vector3 GetHMDPosition()
    {
        return hmdPosition;
    }

    private static Vector3 GetHMDRotation()
    {
        return hmdRotation;
    }
}
