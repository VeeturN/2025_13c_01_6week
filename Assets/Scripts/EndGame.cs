using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // nazwa sceny ustawiana w Inspectorze
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //tutaj zrobimy drugiego if na warunken np score
            SceneManager.LoadScene(sceneToLoad);
        }
        
    }
}