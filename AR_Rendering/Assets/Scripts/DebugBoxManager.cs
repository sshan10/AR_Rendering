using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugBoxManager : MonoBehaviour
{
    public Image DebugBox;
    public TextMeshProUGUI Intensity;
    public TextMeshProUGUI Type;

    public void SetParams(string type, float intensity, Color color)
    {
        this.DebugBox.color = color;
        this.Type.text = type;
        this.Intensity.text = string.Format("{0:0.00}", intensity);
    }

    void Update()
    {
        BillBoard();
    }

    void BillBoard()
    {
        if (Camera.main != null)
        {
            this.transform.LookAt(Camera.main.transform);
        }
    }
}
