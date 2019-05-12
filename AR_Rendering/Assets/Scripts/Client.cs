using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Linq;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System;

using System.Text;

#if !UNITY_EDITOR
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
#endif




/*
 * 
 *  Text Mesh Pro는 동기화를 지원하지 않는다,!
 *  때문에 에러를 유발할 수 있다.
 * 
 */ 




public class Client : MonoBehaviour 
{
    public static Client Instance = null;

    private string SERVER_IP = "";
    private int SERVER_PORT = 6067;

    private Stream reader = null;
    private Stream writer = null;
    private byte[] imageRawData = null;

    private bool serverRunning = true;

    private const int BUFFER_MAX_SIZE = 1024;

    private void Awake()
    {
        Instance = this;
    }        
    
    void Start()
    {
        Connect();
        //DebugWebcamList();
    }

    void OnApplicationQuit()
    {
        Disconnect();   
    }

    public void Connect()
    {
        string ip = "10.20.11.122";
        Debug.Log(string.Format("Server IP: {0}", ip));

        try
        {
            Task.Run(async () =>
            {
                try
                {
#if !UNITY_EDITOR
                    StreamSocket socket = new StreamSocket();
                    await socket.ConnectAsync(new HostName(ip), SERVER_PORT.ToString());
                    reader = socket.InputStream.AsStreamForRead();
                    writer = socket.OutputStream.AsStreamForWrite();
#endif

#if UNITY_EDITOR
                    TcpClient client = new TcpClient(ip, SERVER_PORT);
                    Stream stream = client.GetStream();
                    reader = stream;
                    writer = stream;
#endif
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    return;
                }            

                while (serverRunning)
                {
                    try
                    {
                        byte[] buffer = new byte[sizeof(int)];
                        int nRead = await reader.ReadAsync(buffer, 0, buffer.Length);
                        if (nRead == 0)
                        {
                            break;
                        }

                        int bufferSize = BitConverter.ToInt32(buffer, 0);
                        buffer = new byte[bufferSize];
                        await reader.ReadAsync(buffer, 0, buffer.Length);

                        string data = Encoding.UTF8.GetString(buffer).Trim();
                        InferenceResult result = ParseData(data);
                        Mapping(result);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message + "\n" + e.StackTrace);
                        serverRunning = false;
                        break;
                    }
                }

                if (reader != null)
                {
                    reader.Dispose();
                }

                if (writer != null)
                {
                    writer.Dispose();
                }

            });
        }
        catch(Exception ee)
        {
            reader = null;
            writer = null;
        }
    }

    public void Disconnect()
    {
        serverRunning = false;

        if (writer != null)
        {
            writer.Dispose();
            writer = null;
        }

        if (reader != null)
        {
            reader.Dispose();
            reader = null;
        }
    }

    public void SendToServer(Message message)
    {       
        try
        {
            Task.Run(() =>
            {
                try
                {
                    // sending a message header             
                    byte[] buffer = Encoding.UTF8.GetBytes(message.ToString());
                    int bufferLength = buffer.Length;
                    byte[] bufferSize = BitConverter.GetBytes(bufferLength);

                    writer.Write(bufferSize, 0, bufferSize.Length);
                    writer.Flush();




                    // sending message body 1/2 : hmd position, hmd rotation, image length
                    writer.Write(buffer, 0, buffer.Length);
                    writer.Flush();




                    // sending message body 2/2 : image raw data
                    int remained = message.imageRawData.Length;
                    byte[] bufferParts = null;
                    int index = 0;
                    List<byte> mergedBuffer = message.imageRawData.ToList();

                    while (remained > 0)
                    {
                        bufferLength = (remained > BUFFER_MAX_SIZE) ? BUFFER_MAX_SIZE : remained;
                        bufferParts = mergedBuffer.GetRange(index, bufferLength).ToArray();

                        writer.Write(bufferParts, 0, bufferLength);
                        writer.Flush();

                        index += bufferLength;
                        remained -= bufferLength;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message + "\n" + e.StackTrace);
                    reader = null;
                    writer = null;
                }
            });
        }
        catch (Exception ee)
        {
            Debug.Log(ee.Message + "\n" + ee.StackTrace);
            reader = null;
            writer = null;
        }
    }
    
    byte[] GetImageRawData()
    {
        byte[] rawData = imageRawData != null ? imageRawData : null;

        return rawData;
    }

    public bool IsServerRunning()
    {
        return (reader != null) && (writer != null);
    }

    InferenceResult ParseData(string data)
    {
        string[] parts = data.Split(':');

        string[] hmdPositionRaw = parts[0].Split('|');
        Vector3 hmdPosition = RawToVector3(hmdPositionRaw);

        string[] hmdRotationRaw = parts[1].Split('|');
        Vector3 hmdRotation = RawToVector3(hmdRotationRaw);

        string[] boxes = parts[2].Split('\\');
        DetectionBox[] detectionBoxes = new DetectionBox[boxes.Length];
        for (int i = 0; i < boxes.Length; i++)
        {
            string[] boxData = boxes[i].Split('|');
            detectionBoxes[i] = new DetectionBox(boxData);
        }

        InferenceResult result = new InferenceResult(hmdPosition, hmdRotation, detectionBoxes);
        return result;
    }

    Vector3 RawToVector3(string[] raw)
    {
        Vector3 v = new Vector3()
        {
            x = float.Parse(raw[0]),
            y = float.Parse(raw[1]),
            z = float.Parse(raw[2])
        };

        return v;
    }

    void DebugDetectionBox(DetectionBox[] boxes)
    {
        string result = string.Empty;
        foreach(DetectionBox box in boxes)
        {
            result += string.Format("{0}\n", box.ToDebuggingString());
        }

        Debug.Log(result);
    }

    private void Mapping(InferenceResult result)
    {
        Coordinator.Instance.EnqueueLightData(result);
    }    

    void DebugWebcamList()
    {
        foreach (var device in WebCamTexture.devices)
        {
            Debug.LogFormat("Name: {0}, Kind: {1}", device.name, device.kind.ToString());
        }
    }
}
