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
    [SerializeField] private Image  _item1;
    [SerializeField] private Image  _item2;
    [SerializeField] private Image  _item3;
    [SerializeField] private Image  _item4;
    private Image[] _itemsArray;

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
        Debug.Log("UpdateHUD");
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
