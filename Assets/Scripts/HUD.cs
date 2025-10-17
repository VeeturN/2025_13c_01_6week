using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
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
    private void Awake()
    {
        scoreText.text = "0";
        healthText.text = "10";
        amoText.text = "10";
        strengthPotionCounterText.text = "0";
        hpPotionCounterText.text = "0";
        speedPotionCounterText.text = "0";
        keyCounterText.text = "0";
        _strengthPotion.gameObject.SetActive(false);
        _hpPotion.gameObject.SetActive(false);
        _speedPotion.gameObject.SetActive(false);
        _key.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameEventSystem.OnHUDParameterChanged += UpdateHUD;
    }
    private void OnDestroy()
    {
        GameEventSystem.OnHUDParameterChanged -= UpdateHUD;
    }
    private void UpdateHUD(int currentValue, HUDType hudType)
    {
        switch (hudType)
        {
            case HUDType.Score:
                scoreText.text = currentValue+"";
                break;
            case HUDType.Hp:
                healthText.text = currentValue+"";
                break;
            case HUDType.Ammo:
                amoText.text = currentValue+"";
                break;
            case HUDType.Key:
                keyCounterText.text =  currentValue+"";
                _key.gameObject.SetActive(true);
                break;
            case HUDType.HpPotion:
                hpPotionCounterText.text =  currentValue+"";
                _hpPotion.gameObject.SetActive(true);
                break;
            case HUDType.SpeedPotion:
                speedPotionCounterText.text =  currentValue+"";
                _speedPotion.gameObject.SetActive(true);
                break;
            case HUDType.StrengthPotion:
                strengthPotionCounterText.text =  currentValue+"";
                _strengthPotion.gameObject.SetActive(true);
                break;
        }
    }
}
