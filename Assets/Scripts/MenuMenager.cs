using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

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
    
    [SerializeField] private TextMeshProUGUI _musicOn;
    [SerializeField] private Slider _musicVolumeSlider;
    private bool _slot1 = true;
    private bool _slot2 = true;
    private bool _slot3 = true;
    private bool _slot4 = true;
    [SerializeField] private Button[] _newSlotButtons;
    [SerializeField] private Button[] _LoadSlotButtons;
    [SerializeField] private Button[] _lVLButtons;
    private int _lastClickedSlot = -1;
    private int _OlafSzpontIndex = 0;

    private void Start()
    {
        _MainView.gameObject.SetActive(true);
        _LvlView.HideAtStart();
        _CreditsView.HideAtStart();
        _LoadSaveView.HideAtStart();
        _NewGameSaveView.HideAtStart();
        _ScoreBoardView.HideAtStart();
        _ControlsView.HideAtStart();
        _SettingsView.HideAtStart();
        _AreUSureView.HideAtStart();

        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1f);
        }
        
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("Volume");
        
        if (!isAnySlotTaken())
        {
            PlayerPrefs.DeleteAll();
        }
    }

    #region Main view

    public void StartClicked()
    {
        //tutorial level ale funckja nazwana startclicked z jakiegos powodu
        SceneManager.LoadScene("Tutorial");
        SaveManager.SaveCurrentLevelIndex(SaveManager.GetCurrentSlot(), 420213767);
    }
    public void CreditsClicked()
    {
        _MainView.LeftHide();
        _CreditsView.RightShow();
    }
    public void LoadSaveClicked()
    {
        _MainView.LeftHide();
        _LoadSaveView.RightShow();
        
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
        _NewGameSaveView.RightShow();
        
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
        _ScoreBoardView.RightShow();
    }
    public void ControlsClicked()
    {
        _MainView.LeftHide();
        _ControlsView.RightShow();
    }
    public void SettingsClicked()
    {
        _MainView.LeftHide();
        _SettingsView.RightShow();
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
        _MainView.LeftShow();
        _LvlView.RightHide();
    }

    #endregion
    
    #region Credits view
    public void BackCreditsClicked()
    {
        _MainView.LeftShow();
        _CreditsView.RightHide();
    }

    #endregion
    
    #region LoadSave view
    public void BackLoadSaveClicked()
    {
        _MainView.LeftShow();
        _LoadSaveView.RightHide();
    }
    
    public void SlotClicked(int slotNumber)
    {
        _OlafSzpontIndex=SaveManager.GetCurrentUnlockedLevels(slotNumber);
        SaveManager.SaveCurrentSlot(slotNumber);
        
      //  Debug.Log(SaveManager.GetCurrentUnlockedLevels(slotNumber));
     //   Debug.Log("Loaded game from slot: " + slotNumber);
        
        _LvlView.RightShow();
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
        _MainView.LeftShow();
        _NewGameSaveView.RightHide();
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
      //  Debug.Log("New game started in slot: " + slotNumber);
    }
    private void HideButton()
    {
        _AreUSureView.RightShow();
        _NewGameSaveView.LeftHide();
    }

    #endregion
    
    #region ScoreBoard view
    public void BackScoreBoardClicked()
    {
        _MainView.LeftShow();
        _ScoreBoardView.RightHide();
    }

    #endregion
    
    #region Controls view
    public void BackControlsClicked()
    {
        _MainView.LeftShow();
        _ControlsView.RightHide();
    }

    #endregion
    
    #region Settings view
    public void BackSettingsClicked()
    {
        _MainView.LeftShow();
        _SettingsView.RightHide();
    }
 

    public void OnOffMusic()
    {
        PlayerPrefs.SetInt("Music", PlayerPrefs.GetInt("Music") == 1 ? 0 : 1);
        SoundTrackPlayer.CheckMusic();
        
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            _musicOn.text = "<sprite index=9>";
        }
        else
        {
            _musicOn.text = "<sprite index=4>";
        }
    }
    private void OnMusicVolumeChanged(float value)
    {

        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
        
        SoundTrackPlayer.ChangeVolume(value);
    }

    private void OnDestroy()
    {
        if (_musicVolumeSlider != null)
            _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
    }
    

    #endregion
    
    #region AreUSure view
    public void BackAreUSureClicked()
    {
        _NewGameSaveView.LeftShow();
        _AreUSureView.RightHide();
    }
    public void YesAreUSureClicked()
    {
        _AreUSureView.LeftHide();
        _NewGameSaveView.RightShow();
        UpdateSlotButtonColor(_lastClickedSlot, Color.white);
        SaveManager.DeleteSaveSlot(_lastClickedSlot);
        if (!isAnySlotTaken())
        {
            PlayerPrefs.DeleteAll();
        }
        
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
