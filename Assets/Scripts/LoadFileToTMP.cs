using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadFileToTMP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private string fileName = "mytext.txt";
    [SerializeField] private bool useStreamingAssets = false;
    [SerializeField] private bool createIfMissing = true;

    public void LoadLocal()
    {
        if (textField == null)
        {
            Debug.LogWarning("textField not assigned.");
            return;
        }

        if (useStreamingAssets)
        {
            StartCoroutine(ReadFromStreamingAssets(fileName));
        }
        else
        {
            if (createIfMissing)
                EnsurePersistentFileExists(fileName);

            ReadFromPersistent(fileName);
        }
    }
    private void EnsurePersistentFileExists(string file)
    {
        string directory = GetScriptDirectory();
        string path = Path.Combine(directory, file);

        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        
    }

    private void ReadFromPersistent(string file)
    {
        string directory = GetScriptDirectory();
        string path = Path.Combine(directory, file);

        if (File.Exists(path))
        {
            var lines = File.ReadAllLines(path)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToArray();

            var entries = new List<(string name, int score)>();

            foreach (var line in lines)
            {
                var entry = ParseLineToEntry(line);
                entries.Add(entry);
            }

            // Sort by score descending
            var sorted = entries.OrderByDescending(e => e.score).ToList();

            // Format output: NAZWA (name)  Wynik (score)
            var outputLines = sorted.Select(e => $"{e.name} {e.score}");
            textField.text = string.Join(System.Environment.NewLine, outputLines);
        }
        else
        {
            textField.text = $"FILE NOT FOUND: {path}";
            Debug.LogWarning(textField.text);
        }
    }

    private IEnumerator ReadFromStreamingAssets(string file)
    {
        string path = Path.Combine(Application.streamingAssetsPath, file);

        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (www.result != UnityWebRequest.Result.Success)
#else
        if (www.isNetworkError || www.isHttpError)
#endif
        {
            textField.text = $"Error reading: {path}";
            Debug.LogWarning(www.error);
        }
        else
        {
            // When reading from streaming assets, parse + sort same as persistent
            var raw = www.downloadHandler.text;
            var lines = raw.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            var entries = new List<(string name, int score)>();
            foreach (var line in lines)
                entries.Add(ParseLineToEntry(line));

            var sorted = entries.OrderByDescending(e => e.score);
            var outputLines = sorted.Select(e => $" {e.name} {e.score}");
            textField.text = string.Join(System.Environment.NewLine, outputLines);
        }
    }

    // Try to find the last integer in the line as the score; name is the rest trimmed.
    private (string name, int score) ParseLineToEntry(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return ("NoName", 0);

        // Find last integer in the line
        var matches = Regex.Matches(line, @"-?\d+");
        if (matches.Count > 0)
        {
            var last = matches[matches.Count - 1];
            int score = 0;
            int.TryParse(last.Value, out score);

            // Remove the matched number from the line to get the name/label
            string namePart = line.Remove(last.Index, last.Length).Trim();

            // Remove common separators/labels at end
            namePart = namePart.TrimEnd(':', '-', ' ', '(', ')');
            // Also remove the word "Wynik" if present at end
            namePart = Regex.Replace(namePart, @"\bWynik\b\s*$", "", RegexOptions.IgnoreCase).Trim();

            if (string.IsNullOrEmpty(namePart))
                namePart = "NoName";

            return (namePart, score);
        }
        else
        {
            // No number found — treat whole line as name, score 0
            return (line.Trim(), 0);
        }
    }

    private string GetScriptDirectory()
    {
#if UNITY_EDITOR
        string projectRoot = Path.GetDirectoryName(Application.dataPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        string saveInfoDir = Path.Combine(projectRoot ?? Application.dataPath, "SaveInfo");

        if (!Directory.Exists(saveInfoDir))
            Directory.CreateDirectory(saveInfoDir);

        return saveInfoDir;
#else
        return Application.persistentDataPath;
#endif
    }
}
