using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject _AreUSureView;
    private bool _slot1 = true;
    private bool _slot2 = true;
    private bool _slot3 = true;
    private bool _slot4 = true;
    [SerializeField] private Button[] _newSlotButtons;
    private int _lastClickedSlot = -1;

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
        _AreUSureView.SetActive(false);
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
        
            _slot1=!isSlotTaken(0);
            _slot2=!isSlotTaken(1);
            _slot3=!isSlotTaken(2);
            _slot4=!isSlotTaken(3);

            if (!_slot1)
            {
                UpdateSlotButtonColor(1, Color.red);
            }
            if (!_slot2)
            {
                UpdateSlotButtonColor(2, Color.red);
            }

            if (!_slot3)
            {
                UpdateSlotButtonColor(3, Color.red);
            }
            if (!_slot4)
            {
                UpdateSlotButtonColor(4, Color.red);
            }

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
    
    
    
    private bool isSlotTaken(int slotNumber)
    {
        string folderPath = Application.dataPath + "/../"; 
        string searchPattern = $"slot_{slotNumber}_*"; 
        string[] files = Directory.GetFiles(folderPath, searchPattern);

        if (files.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SlotNewClicked(int slotNumber)
    {
        _lastClickedSlot=slotNumber;
        if (IsSlotAvailable(slotNumber))
            StartNewGame(slotNumber);
        else
            HideButton();

        MarkSlotTaken(slotNumber);
        UpdateSlotButtonColor(slotNumber, Color.red);
    }

    private bool IsSlotAvailable(int slotNumber)
    {
        switch (slotNumber)
        {
            case 1: return _slot1;
            case 2: return _slot2;
            case 3: return _slot3;
            case 4: return _slot4;
            default: return false;
        }
    }

    //Do usuniecia
    private void MarkSlotTaken(int slotNumber)
    {
        switch (slotNumber)
        {
            case 1: _slot1 = false; break;
            case 2: _slot2 = false; break;
            case 3: _slot3 = false; break;
            case 4: _slot4 = false; break;
        }
    }

    private void StartNewGame(int slotNumber)
    {
        _NewGameSaveView.SetActive(false);
        SaveManager.SaveCurrentSlot(slotNumber);
        SaveManager._currentLevelIndex = 1;
        SaveManager._unlockedLevels = 1;
        SceneManager.LoadScene("Jedrek");
        Debug.Log("New game started in slot: " + slotNumber);
    }
    private void HideButton()
    {
        _AreUSureView.SetActive(true);
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
    
    #region AreUSure view
    public void BackAreUSureClicked()
    {
        _NewGameSaveView.SetActive(true);
        _AreUSureView.SetActive(false);
    }
    public void YesAreUSureClicked()
    {
        _AreUSureView.SetActive(false);
        _NewGameSaveView.SetActive(true);
        UpdateSlotButtonColor(_lastClickedSlot, Color.green);
        SaveManager.DeleteSaveSlot(_lastClickedSlot);
        if (_lastClickedSlot==0)
        {
            _slot1=true;
        }
        if (_lastClickedSlot==1)
        {
            _slot2=true;
        }

        if (_lastClickedSlot == 2)
        {
            _slot3=true;
        }
        if (_lastClickedSlot == 3)
        {
            _slot4=true;
        }
    }

    #endregion
    
    private void UpdateSlotButtonColor(int slotNumber, Color color)
    {
        int idx = slotNumber - 1;
        if (_newSlotButtons == null || idx < 0 || idx >= _newSlotButtons.Length) return;
        var img = _newSlotButtons[idx]?.GetComponent<Image>();
        if (img != null) img.color = color;
    }
}
