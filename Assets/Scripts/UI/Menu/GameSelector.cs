using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour
{
    [SerializeField]
    public AvailableGame[] AvailableGames;

    [SerializeField]
    public int CurrentlySelectedGameIndex;

    private LocalizeStringEvent _gameNameLocalizeStringEventComponent;
    private Image _gamePreviewComponent;

    private void Awake()
    {
        _gameNameLocalizeStringEventComponent = transform.Find("LabelGameName").gameObject.GetComponent<LocalizeStringEvent>();
        _gamePreviewComponent = transform.Find("ImageGamePreview").gameObject.GetComponent<Image>();
        SetCurrentlySelectedGame(0);
    }

    public void SetCurrentlySelectedGame(int index)
    {
        int newIndex = CurrentlySelectedGameIndex;

        if (index >= 0 && index < AvailableGames.Length)
        {
            newIndex = index;
        }
        else if(index < 0 && AvailableGames.Length > 1)
        {
            newIndex = AvailableGames.Length - 1;
        }
        else if(index >= AvailableGames.Length && AvailableGames.Length > 1)
        {
            newIndex = 0;
        }

        if(newIndex != CurrentlySelectedGameIndex)
        {
            CurrentlySelectedGameIndex = newIndex;
            LocalizedString localizedStringReference = _gameNameLocalizeStringEventComponent.StringReference;
            localizedStringReference.TableEntryReference = $"Menu.GameNames.{AvailableGames[CurrentlySelectedGameIndex].Name}";
        }
    }

    public void SelectNextGame()
    {
        SetCurrentlySelectedGame(CurrentlySelectedGameIndex - 1);
    }

    public void SelectPreviousGame()
    {
        SetCurrentlySelectedGame(CurrentlySelectedGameIndex + 1);
    }

    public void ProceedToSettingsUIPanel()
    {
        AvailableGames[CurrentlySelectedGameIndex].SettingsUIPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
