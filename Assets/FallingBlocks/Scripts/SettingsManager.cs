using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsManager
{
    public static GlobalSettings GlobalSettings { get; private set; }
    public static FallingBlocksSettings FallingBlocksSettings { get; private set; }

    static SettingsManager()
    {
        GlobalSettings = new GlobalSettings
        {

        };

        FallingBlocksSettings = new FallingBlocksSettings
        {
            FallingBlockLayer = PlayerPrefs.GetInt("FallingBlocksSettings.FallingBlockLayer", (int)EyeLayers.LeftEye),
            ShadowLayer = PlayerPrefs.GetInt("FallingBlocksSettings.ShadowLayer", (int)EyeLayers.Both),
            FallenBlockLayer = PlayerPrefs.GetInt("FallingBlocksSettings.FallenBlockLayer", (int)EyeLayers.RightEye)
        };
    }
}
