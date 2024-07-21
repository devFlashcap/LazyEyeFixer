using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFallingBlockSettings : MonoBehaviour
{
    public TMP_Dropdown DropdownFallingBlockVisibility;
    public TMP_Dropdown DropdownShadowBlockVisibility;
    public TMP_Dropdown DropdownFallenBlockVisibility;

    private void OnEnable()
    {
        DropdownFallingBlockVisibility.value = ToOptionValue(PlayerPrefs.GetInt("FallingBlocksSettings.FallingBlockLayer", (int)EyeLayers.LeftEye));
        DropdownShadowBlockVisibility.value = ToOptionValue(PlayerPrefs.GetInt("FallingBlocksSettings.ShadowLayer", (int)EyeLayers.Both));
        DropdownFallenBlockVisibility.value = ToOptionValue(PlayerPrefs.GetInt("FallingBlocksSettings.FallenBlockLayer", (int)EyeLayers.RightEye));
    }

    private void SaveSettingsChanges()
    {
        PlayerPrefs.SetInt("FallingBlocksSettings.FallingBlockLayer", ToLayerValue(DropdownFallingBlockVisibility.value));
        PlayerPrefs.SetInt("FallingBlocksSettings.ShadowLayer", ToLayerValue(DropdownShadowBlockVisibility.value));
        PlayerPrefs.SetInt("FallingBlocksSettings.FallenBlockLayer", ToLayerValue(DropdownFallenBlockVisibility.value));
        PlayerPrefs.Save();
    }

    private int ToOptionValue(int layer)
    {
        switch (layer)
        {
            case 6:
                return 0;
            case 7: 
                return 1;
            default:
                return 2;
        }
    }

    private int ToLayerValue(int option)
    {
        switch (option)
        {
            case 0:
                return 6;
            case 1:
                return 7;
            default:
                return 0;
        }
    }

    public void Play()
    {
        SaveSettingsChanges();
        SceneManager.LoadScene("FallingBlocks", LoadSceneMode.Single);
    }
}
