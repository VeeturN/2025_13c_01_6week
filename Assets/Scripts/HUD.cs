using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    private int maxHp = 10;
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
    }
    private void Start()
    {
        GameEventSystem.OnHUDParameterChanged += UpdateHUD;
        GameEventSystem.OnMapFragmentCollected += UpdateSecretMapsField;
        GameEventSystem.OnPotionTimeChaged += UpdatePotionBar;
    }
    private void OnDestroy()
    {
        GameEventSystem.OnHUDParameterChanged -= UpdateHUD;
        GameEventSystem.OnPotionTimeChaged -= UpdatePotionBar;
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
}
