using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using System;

public class ImageUtil
{
    public static Texture2D RawToTexture2D(byte[] rawdata)
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(rawdata);

        return tex;
    }

    public static Sprite TextureToSprite(Texture2D tex)
    {
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(.5f, .5f));

        return sprite;
    }

    public static byte[] TextureToRawdata(Texture2D texture)
    {
        return texture.EncodeToJPG(100);
    }
}
