using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] float _dialogDelay = 0.07f;
    SpriteRenderer _dialogBorder;
    TextMeshPro _textMeshPro;

    void Awake()
    {
        _dialogBorder = GetComponentInParent<SpriteRenderer>();
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    public void Init(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TypeCoroutine(text ?? string.Empty, _dialogDelay));
    }

    private IEnumerator TypeCoroutine(string text, float delay)
    {
        text = text.Replace("\\n", "\n").Replace("/n", "\n");
        text = text.ToUpper();
        text = text.Replace("\n", " \n ");

        _textMeshPro.text = "";
        _textMeshPro.ForceMeshUpdate();

        string[] words = text.Split(' ');
        foreach (var w in words)
        {
            if (string.IsNullOrEmpty(w)) continue;

            if (w == "\n")
            {
                _textMeshPro.text += "\n";
                _textMeshPro.ForceMeshUpdate();
                var b1 = _textMeshPro.textBounds.size;
                if (_dialogBorder != null) _dialogBorder.size = new Vector2(b1.x + 0.3f, b1.y + 0.4f);
                yield return null;
                continue;
            }

            if (_textMeshPro.text.Length > 0 && !_textMeshPro.text.EndsWith("\n"))
                _textMeshPro.text += " ";

            foreach (char c in w)
            {
                _textMeshPro.text += c;
                _textMeshPro.ForceMeshUpdate();
                var b = _textMeshPro.textBounds.size;
                if (_dialogBorder != null) _dialogBorder.size = new Vector2(b.x + 0.3f, b.y + 0.4f);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
