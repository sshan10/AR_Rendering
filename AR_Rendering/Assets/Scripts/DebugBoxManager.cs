using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugBoxManager : MonoBehaviour
{
    public Image Box;
    public TextMeshProUGUI Intensity;
    public TextMeshProUGUI Index;
    public TextMeshProUGUI Type;

    public void SetParams(int id, float intensity, Color color)
    {
        this.Index.text = id.ToString();
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
