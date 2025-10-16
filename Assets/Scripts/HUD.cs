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
        GameEventSystem.OnPlayerScoreUpdated += updateScore;
    }

    private void OnDestroy()
    {
        GameEventSystem.OnPlayerScoreUpdated -= updateScore;
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
    

}
