using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMenager : MonoBehaviour
{
    [SerializeField] private GameObject _MainView;
    [SerializeField] private GameObject _LvlView;
    [SerializeField] private GameObject _CreditsView;
    [SerializeField] private GameObject _LoadSaveView;
    [SerializeField] private GameObject _NewGameSaveView;
    [SerializeField] private GameObject _ScoreBoardView;
    [SerializeField] private GameObject _ControlsView;
    [SerializeField] private GameObject _SettingsView;

    public void Awake()
    {
        _MainView.SetActive(true);
        _LvlView.SetActive(false);
        _CreditsView.SetActive(false);
        _LoadSaveView.SetActive(false);
        _NewGameSaveView.SetActive(false);
        _ScoreBoardView.SetActive(false);
        _ControlsView.SetActive(false);
        _SettingsView.SetActive(false);
    }

    #region Main view

    public void StartClicked()
    {
        _MainView.SetActive(false);
        _LvlView.SetActive(true);
    }
    public void CreditsClicked()
    {
        _MainView.SetActive(false);
        _CreditsView.SetActive(true);
    }
    public void LoadSaveClicked()
    {
        _MainView.SetActive(false);
        _LoadSaveView.SetActive(true);
    }
    public void NewSaveClicked()
    {
        _MainView.SetActive(false);
        _NewGameSaveView.SetActive(true);
    }
    public void ScoreBoardClicked()
    {
        _MainView.SetActive(false);
        _ScoreBoardView.SetActive(true);
    }
    public void ControlsClicked()
    {
        _MainView.SetActive(false);
        _ControlsView.SetActive(true);
    }
    public void SettingsClicked()
    {
        _MainView.SetActive(false);
        _SettingsView.SetActive(true);
    }

    public void ExitClicked()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #endregion
    
    #region Lvl view

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void BackLvlClicked()
    {
        _MainView.SetActive(true);
        _LvlView.SetActive(false);
    }

    #endregion
    
    #region Credits view
    public void BackCreditsClicked()
    {
        _MainView.SetActive(true);
        _CreditsView.SetActive(false);
    }

    #endregion
    
    #region LoadSave view
    public void BackLoadSaveClicked()
    {
        _MainView.SetActive(true);
        _LoadSaveView.SetActive(false);
    }
    
    public void SlotClicked()
    {
        _LvlView.SetActive(true);
        _LoadSaveView.SetActive(false);
    }

    #endregion
    
    #region NewGameSlot view
    public void BackNewGameSlotClicked()
    {
        _MainView.SetActive(true);
        _NewGameSaveView.SetActive(false);
    }
    
    public void SlotNewClicked()
    {
        _LvlView.SetActive(true);
        _NewGameSaveView.SetActive(false);
    }

    #endregion
    
    #region ScoreBoard view
    public void BackScoreBoardClicked()
    {
        _MainView.SetActive(true);
        _ScoreBoardView.SetActive(false);
    }

    #endregion
    
    #region Controls view
    public void BackControlsClicked()
    {
        _MainView.SetActive(true);
        _ControlsView.SetActive(false);
    }

    #endregion
    
    #region Controls view
    public void BackSettingsClicked()
    {
        _MainView.SetActive(true);
        _SettingsView.SetActive(false);
    }

    #endregion
}
