using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI amoText;
    [SerializeField] private Image  _strengthPotion;
    [SerializeField] private Image  _hpPotion;
    [SerializeField] private Image  _speedPotion;
    [SerializeField] private Image  _key;
    [SerializeField] private TextMeshProUGUI strengthPotionCounterText;
    [SerializeField] private TextMeshProUGUI hpPotionCounterText;
    [SerializeField] private TextMeshProUGUI speedPotionCounterText;
    [SerializeField] private TextMeshProUGUI keyCounterText;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpSpeed;
    [SerializeField] private Image hpStrenght;
    [SerializeField] private Image secretMapTopRight;
    [SerializeField] private Image secretMapTopLeft;
    [SerializeField] private Image secretMapBottomRight;
    [SerializeField] private Image secretMapBottomLeft;
    
    [SerializeField] private GameObject Exit;
    [SerializeField] private GameObject ScoreBoardView;
    [SerializeField] private ShopScript GameOverView;

    private int maxHp = 10;

    protected float _savedTimeScale = 1f;
    protected float _savedFixedDeltaTime = 0.02f;
    
    private void Awake()
    {
        scoreText.text = "0";
        amoText.text = "10";
        strengthPotionCounterText.text = "0";
        hpPotionCounterText.text = "0";
        speedPotionCounterText.text = "0";
        keyCounterText.text = "0";
        hpBar.fillAmount = 1f;
        hpSpeed.fillAmount = 0f;
        hpStrenght.fillAmount = 0f;
        
        secretMapTopRight.enabled = false;
        secretMapTopLeft.enabled = false;
        secretMapBottomRight.enabled = false;
        secretMapBottomLeft.enabled = false;
        
        
        GameEventSystem.OnHUDParameterChanged += UpdateHUD;
        GameEventSystem.OnMapFragmentCollected += UpdateSecretMapsField;
        GameEventSystem.OnPotionTimeChaged += UpdatePotionBar;
        GameEventSystem.OnPlayerDeath += ShowDeathMenu;
    }

    private void ShowDeathMenu()
    {
        GameOverView.DownShow();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ScoreBoardView.activeSelf)
        {

            bool willOpen = !Exit.activeSelf;
            Exit.SetActive(willOpen);

            if (willOpen) 
            {
                _savedTimeScale = Time.timeScale;
                _savedFixedDeltaTime = Time.fixedDeltaTime;
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0f;
                AudioListener.pause = true;
            }
            else 
            {
                Time.timeScale = _savedTimeScale > 0f ? _savedTimeScale : 1f;
                Time.fixedDeltaTime = _savedFixedDeltaTime > 0f ? _savedFixedDeltaTime : 0.02f;
                AudioListener.pause = false;
            }
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.OnHUDParameterChanged -= UpdateHUD;
        GameEventSystem.OnPotionTimeChaged -= UpdatePotionBar;
        GameEventSystem.OnMapFragmentCollected -= UpdateSecretMapsField;
        GameEventSystem.OnPlayerDeath -= ShowDeathMenu;
    }
    private void UpdateSecretMapsField(SecretMapFragment secretMapFragment)
    {
        switch (secretMapFragment.GetMapFragmentEnum())
        {
            case MapFragmentEnum.TopLeft:
                secretMapTopLeft.enabled = true;
                break;
            case MapFragmentEnum.TopRight:
                secretMapTopRight.enabled = true;
                break;
            case MapFragmentEnum.BottomRight:
                secretMapBottomRight.enabled = true;
                break;
            case MapFragmentEnum.BottomLeft:
                secretMapBottomLeft.enabled = true;
                break;
        }
        
    }
    private void UpdateHUD(int currentValue, HUDType hudType)
    {
        switch (hudType)
        {
            case HUDType.Score:
                scoreText.text = currentValue+"";
                break;
            case HUDType.Hp:
                hpBar.fillAmount = (float)currentValue/maxHp;
                Debug.Log($"HP currnet: {currentValue}/{maxHp}");
                break;
            case HUDType.Ammo:
                amoText.text = currentValue+"";
                break;
            case HUDType.Key:
                keyCounterText.text =  currentValue+"";
                break;
            case HUDType.HpPotion:
                hpPotionCounterText.text =  currentValue+"";
                break;
            case HUDType.SpeedPotion:
                speedPotionCounterText.text =  currentValue+"";
                break;
            case HUDType.StrengthPotion:
                strengthPotionCounterText.text =  currentValue+"";
                break;
        }
    }
    private void UpdatePotionBar(float x, PotionEnum y)
    {
        switch (y)
        {
            case PotionEnum.Blue:
                hpSpeed.fillAmount = x;
                break;
            case PotionEnum.Green:
                hpStrenght.fillAmount = x;
                break;
        }
    }
    
    public void Resume()
    {
        if (Exit.activeSelf)
            Exit.SetActive(false);
        
        Time.timeScale = _savedTimeScale > 0f ? _savedTimeScale : 1f;
        Time.fixedDeltaTime = _savedFixedDeltaTime > 0f ? _savedFixedDeltaTime : 0.02f;
        AudioListener.pause = false;
    }
    
    public void ExitToMainMenu()
    {

        if (Exit != null && Exit.activeSelf)
            Exit.SetActive(false);

        Time.timeScale = _savedTimeScale > 0f ? _savedTimeScale : 1f;
        Time.fixedDeltaTime = _savedFixedDeltaTime > 0f ? _savedFixedDeltaTime : 0.02f;
        AudioListener.pause = false;

        SceneManager.LoadScene("MainMenu");
    }
    public void LoadLastCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopAndShowScoreBoard()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        AudioListener.pause = true;
        ScoreBoardView.SetActive(true);
    }

    public int GetScore()
    {
        scoreText.text = scoreText.text.Trim();
        if (int.TryParse(scoreText.text, out int score)) 
            return score;
        return 0;
    }

}
