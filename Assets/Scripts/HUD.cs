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
    
    
    void Awake()
    {
        _itemsArray = new Image[] { _item1, _item2, _item3, _item4 };
    }
    
    public void updateScore(int currentScore)
    {
        scoreText.text = currentScore+"";
    }

    public void updateHealth(int currentHealth)
    {
        healthText.text = currentHealth+"";
    }

    public void updateAmo(int currentAmo)
    {
        amoText.text = currentAmo + "";
    }

    public void updateInventory(Sprite[] itemSpritesArray)
    {
        //tu dodac zeby jako parametr funkcji przekazywac np arreya typu przedmiot, zeby to mozna bylo normalnie z tego
        //poziomu updatowac
       
        // Iteruj po tablicy i przypisz sprite'y do Image
        for (int i = 0; i < _itemsArray.Length && i < itemSpritesArray.Length; i++)
        {
            if (_itemsArray[i] != null && itemSpritesArray[i] != null)
            {
                _itemsArray[i].sprite = itemSpritesArray[i];
                _itemsArray[i].enabled = true; // Pokaż obrazek
            }
        }
        
        // Ukryj pozostałe sloty jeśli nie ma wystarczająco przedmiotów
        for (int i = itemSpritesArray.Length; i < _itemsArray.Length; i++)
        {
            if (_itemsArray[i] != null)
            {
                _itemsArray[i].enabled = false; // Ukryj pusty slot
            }
        }
        
    }

}
