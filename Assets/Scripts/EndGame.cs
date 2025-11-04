using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //jesli odblokujemy nowy poziom ale jeszce bez sprawdzenia
            //i jednoczesnie zapisz poziom
            SaveManager.SaveGameStateDataXML();
            SaveManager.SaveLevelDataXML(SaveManager._currentLevelIndex, other.transform.position);
            
            int currSlot = SaveManager.GetCurrentSlot();
            int juzOdblowkowane = SaveManager.GetCurrentUnlockedLevels(currSlot);
            
            SaveManager.SaveCurrentUnlockedLevels(currSlot, juzOdblowkowane+1);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}