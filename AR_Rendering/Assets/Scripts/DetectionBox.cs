using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBox
{
    private const int ID_INDEX = 0;
    private const int SCORE_INDEX = 1;
    private const int MIN_X_INDEX = 2;
    private const int MIN_Y_INDEX = 3;
    private const int MAX_X_INDEX = 4;
    private const int MAX_Y_INDEX = 5;
    private const int COLOR_R_INDEX = 6;
    private const int COLOR_G_INDEX = 7;
    private const int COLOR_B_INDEX = 8;
    private const int INTENSITY_INDEX = 9;


    public int id;
    public float score;
    public Vector2 min;
    public Vector2 max;
    public Color color;
    public float intensity;

    public DetectionBox()
    {
        id = 0;
        score = 0f;
        min = max = Vector2.zero;
        color = Color.black;
        intensity = 0f;
    }

    public DetectionBox(string[] rawData)
    {
        this.id = int.Parse(rawData[ID_INDEX]);
        this.score = float.Parse(rawData[SCORE_INDEX]);
        this.min = new Vector2(float.Parse(rawData[MIN_X_INDEX]), float.Parse(rawData[MIN_Y_INDEX]));
        this.max = new Vector2(float.Parse(rawData[MAX_X_INDEX]), float.Parse(rawData[MAX_Y_INDEX]));
        this.color = new Color(float.Parse(rawData[COLOR_R_INDEX]), float.Parse(rawData[COLOR_G_INDEX]), float.Parse(rawData[COLOR_B_INDEX]));
        this.intensity = float.Parse(rawData[INTENSITY_INDEX]);
    }

    public override string ToString()
    {
        string text = string.Empty;

        text += string.Format("{0}|", id);
        text += string.Format("{0}|", score);
        text += string.Format("{0}|{1}|", min.x, min.y);
        text += string.Format("{0}|{1}|", max.x, max.y);
        text += string.Format("{0}|{1}|{2}|", color.r, color.g, color.b);
        text += string.Format("{0}", intensity);

        return text;
    }
}