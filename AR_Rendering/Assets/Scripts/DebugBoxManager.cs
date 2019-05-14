using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugBoxManager : MonoBehaviour
{
    public Renderer SphereRenderer;
    public Material originMat;
    public TextMeshProUGUI Intensity;
    public TextMeshProUGUI Type;

    public void SetParams(string type, float intensity, Color color)
    {
        this.Type.text = type;
        this.Intensity.text = string.Format("{0:0.00}", intensity);

        Material newMat = new Material(originMat);
        newMat.color = color;
        SphereRenderer.material = newMat;
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
