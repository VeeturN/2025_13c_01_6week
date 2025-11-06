using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMenager : MonoBehaviour
{
    [SerializeField] private MainMenuPopup _MainView;
    [SerializeField] private MainMenuPopup _LvlView;
    [SerializeField] private MainMenuPopup _CreditsView;
    [SerializeField] private MainMenuPopup _LoadSaveView;
    [SerializeField] private MainMenuPopup _NewGameSaveView;
    [SerializeField] private MainMenuPopup _ScoreBoardView;
    [SerializeField] private MainMenuPopup _ControlsView;
    [SerializeField] private MainMenuPopup _SettingsView;
    [SerializeField] private MainMenuPopup _AreUSureView;
    private bool _slot1 = true;
    private bool _slot2 = true;
    private bool _slot3 = true;
    private bool _slot4 = true;
    [SerializeField] private Button[] _newSlotButtons;
    [SerializeField] private Button[] _LoadSlotButtons;
    [SerializeField] private Button[] _lVLButtons;
    private int _lastClickedSlot = -1;
    private int _OlafSzpontIndex = 0;

    public void Awake()
    {
        _MainView.gameObject.SetActive(true);
        _LvlView.gameObject.SetActive(false);
        _CreditsView.gameObject.SetActive(false);
        _LoadSaveView.gameObject.SetActive(false);
        _NewGameSaveView.gameObject.SetActive(false);
        _ScoreBoardView.gameObject.SetActive(false);
        _ControlsView.gameObject.SetActive(false);
        _SettingsView.gameObject.SetActive(false);
        _AreUSureView.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (!isAnySlotTaken())
        {
            PlayerPrefs.DeleteAll();
        }
    }

    #region Main view

    public void StartClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void CreditsClicked()
    {
        _MainView.LeftHide();
        _CreditsView.gameObject.SetActive(true);
    }
    public void LoadSaveClicked()
    {
        _MainView.LeftHide();
        _LoadSaveView.gameObject.SetActive(true);
        
            _slot1=!IsSlotTaken(1);
            _slot2=!IsSlotTaken(2);
            _slot3=!IsSlotTaken(3);
            _slot4=!IsSlotTaken(4);

            if (!_slot1)
            {
                _LoadSlotButtons[0].interactable = true;
            }
            if (!_slot2)
            {
                _LoadSlotButtons[1].interactable = true;
            }

            if (!_slot3)
            {
                _LoadSlotButtons[2].interactable = true;
            }
            if (!_slot4)
            {
                _LoadSlotButtons[3].interactable = true;
            }
    }
    public void NewSaveClicked()
    {
        _MainView.LeftHide();
        _NewGameSaveView.gameObject.SetActive(true);
        
            _slot1=!IsSlotTaken(1);
            _slot2=!IsSlotTaken(2);
            _slot3=!IsSlotTaken(3);
            _slot4=!IsSlotTaken(4);

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
        _MainView.LeftHide();
        _ScoreBoardView.gameObject.SetActive(true);
    }
    public void ControlsClicked()
    {
        _MainView.LeftHide();
        _ControlsView.gameObject.SetActive(true);
    }
    public void SettingsClicked()
    {
        _MainView.LeftHide();
        _SettingsView.gameObject.SetActive(true);
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
    
    
    private int ExtractLevelIndex(string levelName)
    {
        Match match = Regex.Match(levelName, @"\d+");
        if (match.Success)
        {
          
            return int.Parse(match.Value);
        }
        return 0;
    }
    public void LoadLevel(string levelName)
    {
        int levelIndex = ExtractLevelIndex(levelName);
        SceneManager.LoadScene(levelName);
        SaveManager.SaveCurrentLevelIndex(SaveManager.GetCurrentSlot(),levelIndex);
    }

    public void BackLvlClicked()
    {
        _MainView.gameObject.SetActive(true);
        _LvlView.LeftHide();
    }

    #endregion
    
    #region Credits view
    public void BackCreditsClicked()
    {
        _MainView.gameObject.SetActive(true);
        _CreditsView.LeftHide();
    }

    #endregion
    
    #region LoadSave view
    public void BackLoadSaveClicked()
    {
        _MainView.gameObject.SetActive(true);
        _LoadSaveView.LeftHide();
    }
    
    public void SlotClicked(int slotNumber)
    {
        _OlafSzpontIndex=SaveManager.GetCurrentUnlockedLevels(slotNumber);
        Debug.Log(SaveManager.GetCurrentUnlockedLevels(slotNumber));
        Debug.Log("Loaded game from slot: " + slotNumber);
        
        _LvlView.gameObject.SetActive(true);
        _LoadSaveView.LeftHide();
        
        for (int i = 0; i < _lVLButtons.Length; i++)
        {
            if (i < _OlafSzpontIndex)
            {
                _lVLButtons[i].interactable = true;
            }
            else
            {
                _lVLButtons[i].interactable = false;
            }
        }
    }
    
    

    #endregion
    
    #region NewGameSlot view
    public void BackNewGameSlotClicked()
    {
        _MainView.gameObject.SetActive(true);
        _NewGameSaveView.LeftHide();
    }
    
    
    
    private bool IsSlotTaken(int slotNumber)
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

    private bool isAnySlotTaken()
    {
        string folderPath = Application.dataPath + "/../"; 
        string searchPattern = $"slot_*"; 
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
        _NewGameSaveView.LeftHide();
        SaveManager.SaveCurrentSlot(slotNumber);
        SaveManager.SaveCurrentLevelIndex(slotNumber, 1);
        SceneManager.LoadScene("Level1");
        Debug.Log("New game started in slot: " + slotNumber);
    }
    private void HideButton()
    {
        _AreUSureView.gameObject.SetActive(true);
        _NewGameSaveView.LeftHide();
    }

    #endregion
    
    #region ScoreBoard view
    public void BackScoreBoardClicked()
    {
        _MainView.gameObject.SetActive(true);
        _ScoreBoardView.LeftHide();
    }

    #endregion
    
    #region Controls view
    public void BackControlsClicked()
    {
        _MainView.gameObject.SetActive(true);
        _ControlsView.LeftHide();
    }

    #endregion
    
    #region Settings view
    public void BackSettingsClicked()
    {
        _MainView.gameObject.SetActive(true);
        _SettingsView.LeftHide();
    }

    #endregion
    
    #region AreUSure view
    public void BackAreUSureClicked()
    {
        _NewGameSaveView.gameObject.SetActive(true);
        _AreUSureView.LeftHide();
    }
    public void YesAreUSureClicked()
    {
        _AreUSureView.LeftHide();
        _NewGameSaveView.gameObject.SetActive(true);
        UpdateSlotButtonColor(_lastClickedSlot, Color.white);
        SaveManager.DeleteSaveSlot(_lastClickedSlot);
        if (_lastClickedSlot==1)
        {
            _slot1=true;
        }
        if (_lastClickedSlot==2)
        {
            _slot2=true;
        }

        if (_lastClickedSlot == 3)
        {
            _slot3=true;
        }
        if (_lastClickedSlot == 4)
        {
            _slot4=true;
        }
    }

    #endregion
    
    private void UpdateSlotButtonColor(int slotNumber, Color color)
    {
        int idx = slotNumber - 1;
        var img = _newSlotButtons[idx]?.GetComponent<Image>();
        if (img != null) img.color = color;
    }
}
