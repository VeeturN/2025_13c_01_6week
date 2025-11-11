using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] Slot[] _slots;
    [SerializeField] TextMeshProUGUI _buttonText;
    [SerializeField] Button _spinButton;
    [SerializeField] Button _collectButton;
    private bool _isSpinning = false;
    public void Start()
    {
        _isSpinning = false;
        _buttonText.text = "SPIN";
    }

    public void Update()
    {
        bool canBeCollected = true;
        foreach(Slot slot in _slots)
        {
            if(!slot.CanBeCollected)
                canBeCollected = false;
        }
        if (canBeCollected)
            _collectButton.gameObject.SetActive(true);
    }
    public void SwitchSpin()
    {
        if (!_isSpinning)
        {
            if (SaveManager.GetCurrentLevel(SaveManager.GetCurrentSlot()) != 420213767)
            {
                if (Inventory.Score >= 400)
                {
                    Inventory.Score -= 400;
                    GameEventSystem.UpdateHUD(Inventory.Score, HUDType.Score);
                }
                else
                {
                    Debug.Log("Jestes za biedny na automaty");
                    return;
                }
            }
          
         
            foreach (Slot slot in _slots)
            {
                slot.StartSpin();
            }
            _isSpinning = true;
            _buttonText.text = "STOP";
        }
        else
        {
            foreach (Slot slot in _slots)
            {
                slot.StopSpin();
            }
            _isSpinning=false;
            _buttonText.text = "SPIN";
            _spinButton.gameObject.SetActive(false);
            //_collectButton.gameObject.SetActive(true);

        }
    }
    public void Collect()
    {
        foreach (Slot slot in _slots)
        {
            slot.Collect();
        }
        _spinButton.gameObject.SetActive(true);
        _collectButton.gameObject.SetActive(false);
    }
}
