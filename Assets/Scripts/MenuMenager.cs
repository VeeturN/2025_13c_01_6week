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

    public void Awake()
    {
        _MainView.SetActive(true);
        _LvlView.SetActive(false);
    }

    #region Main view

    public void StartClicked()
    {
        _MainView.SetActive(false);
        _LvlView.SetActive(true);
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

    public void BackClicked()
    {
        _MainView.SetActive(true);
        _LvlView.SetActive(false);
    }

    #endregion
}
