using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int currSlot = SaveManager.GetCurrentSlot();
            int juzOdblowkowane = SaveManager.GetCurrentUnlockedLevels(currSlot);

            if (SaveManager.GetCurrentLevel(currSlot) < juzOdblowkowane)
            {
                SaveManager.SaveCurrentLevelIndex(currSlot,SaveManager.GetCurrentLevel(currSlot)+1);
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                SaveManager.SaveCurrentUnlockedLevels(currSlot, SaveManager.GetCurrentLevel(currSlot)+1);
                SaveManager.SaveCurrentLevelIndex(currSlot,SaveManager.GetCurrentLevel(currSlot)+1);
                SceneManager.LoadScene(sceneToLoad);
                Debug.Log(SaveManager.GetCurrentUnlockedLevels(currSlot));
            }
        }
    }
}