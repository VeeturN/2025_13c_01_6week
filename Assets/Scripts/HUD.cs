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
    private Image[] _itemsArray;

    private void Awake()
    {
        scoreText.text = "0";
        healthText.text = "10";
        amoText.text = "10";
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
        }
    }
}
