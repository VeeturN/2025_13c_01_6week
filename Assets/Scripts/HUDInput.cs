using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private string fileName = "mytext.txt";
    private string score = "";
    private int scoreInt = 0;

    private void Awake()
    {
        if (inputField == null)
            inputField = GetComponentInChildren<TMP_InputField>();
    }

    private void Start()
    {
        if (inputField != null)
            inputField.onEndEdit.AddListener(OnSubmit);
    }

    private void OnDestroy()
    {
        if (inputField != null)
            inputField.onEndEdit.RemoveListener(OnSubmit);
    }

    public void OnSubmit(string text)
    {
        score = text.Trim();
        Debug.Log("Input submitted: " + score);
    }

    public void AddScore()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        AudioListener.pause = false;
        if (string.IsNullOrEmpty(score))
        {
            score="NoName";
        }

        string saveDir = GetSaveDirectory();
        string path = Path.Combine(saveDir, fileName);

        try
        {
            // Append the score + newline to the file
            File.AppendAllText(path, score + System.Environment.NewLine);
            Debug.Log($"Appended score to: {path}");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Failed to write score to {path}: {ex.Message}");
        }

        SceneManager.LoadScene("MainMenu");
    }

    private string GetSaveDirectory()
    {
#if UNITY_EDITOR

        string projectRoot = Path.GetDirectoryName(Application.dataPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        string saveInfoDir = Path.Combine(projectRoot ?? Application.dataPath, "SaveInfo");

        if (!Directory.Exists(saveInfoDir))
            Directory.CreateDirectory(saveInfoDir);

        return saveInfoDir;
#else
        // In builds use persistentDataPath
        return Application.persistentDataPath;
#endif
    }
}
