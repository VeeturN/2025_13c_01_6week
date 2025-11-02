using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] float _dialogDelay = 0.07f;
    SpriteRenderer _dialogBorder;
    private TextMeshPro _textMeshPro;

    void Awake()
    {
        _dialogBorder = GetComponentInParent<SpriteRenderer>();
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    public void Init(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TypeCoroutine(text, _dialogDelay));
    }

    private IEnumerator TypeCoroutine(string text, float delay)
    {
        text = text.ToUpper();
        _textMeshPro.text = text;
        _textMeshPro.ForceMeshUpdate();
        Vector3 fullBounds = _textMeshPro.textBounds.size;
        transform.parent.position = transform.parent.parent.position + Vector3.up * fullBounds.y+0.8f*Vector3.up;
        _textMeshPro.text = "";


        string[] words = text.Split(' ');
        foreach (string word in words)
        {
            string prevText = _textMeshPro.text;

            _textMeshPro.text = prevText + " " + word;
            _textMeshPro.ForceMeshUpdate();
            int newLineCount = _textMeshPro.textInfo.lineCount;

            {
                Vector3 bounds = _textMeshPro.textBounds.size;
                _dialogBorder.size = new Vector2(bounds.x + 0.3f, bounds.y + 0.4f);
            }

            _textMeshPro.text = prevText;
            _textMeshPro.ForceMeshUpdate();
            int oldLineCount = _textMeshPro.textInfo.lineCount;
            if (_textMeshPro.text!="")
            {
                if (newLineCount > oldLineCount)
                    _textMeshPro.text += "\n";
                else
                    _textMeshPro.text += " ";
            }

                foreach (char c in word)
                {
                    _textMeshPro.text += c;
                    yield return new WaitForSeconds(delay);
                }
        }
    }
}
