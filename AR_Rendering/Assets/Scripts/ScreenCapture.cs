using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;

public class ScreenCapture : MonoBehaviour
{    
    public static bool capturing = false;

    PhotoCapture photoCaptureObject = null;
    Texture2D capturedTexture = null;

    private Vector3 hmdPosition, hmdRotation;
    
    public void Capture()
    {
        capturing = true;

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

    void OnCapturePhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    { 
        if(capturedTexture == null)
        {
            return;
        }

        photoCaptureFrame.UploadImageDataToTexture(capturedTexture);
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);

        Message message = new Message()
        {
            hmdPosition = GetHMDPosition(),
            hmdRotation = GetHMDRotation(),
            imageRawData = TextureToRawdata(capturedTexture)
        };

        if (Client.Instance != null &&  Client.Instance.IsServerRunning())
        {
            Client.Instance.SendToServer(message);
        }

        capturing = false;
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private void SetHMDTransform()
    {
        hmdPosition = Camera.main.transform.position;
        hmdRotation = Camera.main.transform.eulerAngles;
    }
    
    private Vector3 GetHMDPosition()
    {
        return hmdPosition;
    }

    private Vector3 GetHMDRotation()
    {
        return hmdRotation;
    }

    private byte[] TextureToRawdata(Texture2D texture)
    {
        return texture.EncodeToJPG(100);
    }
}
