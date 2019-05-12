using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugBoxManager : MonoBehaviour
{
    public Image Box;
    public TextMeshProUGUI Intensity;
    public TextMeshProUGUI Type;

    public void SetParams(LightType type, float intensity, Color color)
    {
        this.Type.text = type.ToString();
        this.Intensity.text = string.Format("{0:0.00}", intensity);

        this.Box.color = color;
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
