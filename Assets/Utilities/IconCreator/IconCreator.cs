using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class IconCreator : MonoBehaviour
{
    public string SpriteName;

    public bool Create;
    public RenderTexture MyRenderTexture;
    public Camera BakeCamera;

    void Update()
    {
        if(Create)
        {
            Create = false;
            CreateIcon();
        }
    }

    void CreateIcon()
    {
        if(string.IsNullOrEmpty(SpriteName))
        {
            SpriteName = "icon";
        }

        string path = SaveLocation() + SpriteName;
        BakeCamera.targetTexture = MyRenderTexture;

        RenderTexture currentRenderTexture = RenderTexture.active;
        BakeCamera.targetTexture.Release();
        RenderTexture.active = BakeCamera.targetTexture;
        BakeCamera.Render();

        Texture2D bakedPNG = new Texture2D(BakeCamera.targetTexture.width, BakeCamera.targetTexture.height, TextureFormat.ARGB32, false);
        bakedPNG.ReadPixels(new Rect(0, 0, BakeCamera.targetTexture.width, BakeCamera.targetTexture.height), 0, 0);
        bakedPNG.Apply();
        RenderTexture.active = currentRenderTexture;
        byte[] bakedPNGBytes = bakedPNG.EncodeToPNG();
        System.IO.File.WriteAllBytes($"{path}.png", bakedPNGBytes);
        Debug.Log($"Icon created: {path}.png");
    }

    string SaveLocation()
    {
        string saveLocation = Application.streamingAssetsPath + "/Utilities/IconCreator/Icons/";
        if(!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }
}
