using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HUD : MonoBehaviour

{
    
   
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void updateScore(int currentScore)
    {
        scoreText.text = currentScore+"";
    }

    public void updateHealth(int currentHealth)
    {
        healthText.text = currentHealth+"";
    } 
    
}
