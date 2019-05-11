using UnityEngine;
using System.Linq;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.UI;

using TMPro;

public class ScreenCapture : MonoBehaviour
{
    /*Debug variable*/
    public TextMeshProUGUI DebugText;
    /*Debug variable end*/
    
    public static bool capturing = false;

    public Image image;

    PhotoCapture photoCaptureObject = null;
    Texture2D capturedTexture = null;

    
    public void Capture()
    {
        capturing = true;

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

        image.sprite = ImageUtil.TextureToSprite(capturedTexture);

        capturedTexture = null;

        capturing = false;
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
    
    private Vector3 GetHMDPosition()
    {
        if(Camera.main != null)
        {
            return Camera.main.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 GetHMDRotation()
    {
        if (Camera.main != null)
        {
            return Camera.main.transform.eulerAngles;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private byte[] TextureToRawdata(Texture2D texture)
    {
        return texture.EncodeToJPG(100);
    }
}
