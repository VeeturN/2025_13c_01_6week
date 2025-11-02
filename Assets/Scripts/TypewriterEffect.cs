using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    SpriteRenderer _dialogBorder;
    private TextMeshPro _textMeshPro;

    void Awake()
    {
        _dialogBorder = GetComponentInParent<SpriteRenderer>();
        _textMeshPro = GetComponent<TextMeshPro>();

        string dialog = "PRESS A OR D TO MOVE AROUND";
        StartCoroutine(TypeCoroutine(dialog, 0.1f));
    }

    private IEnumerator TypeCoroutine(string text, float delay)
    {
        _textMeshPro.text = text;
        _textMeshPro.ForceMeshUpdate();
        Vector3 fullBounds = _textMeshPro.textBounds.size;
        transform.parent.position += Vector3.up * fullBounds.y/2;
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

           // if (newLineCount > oldLineCount)
            //{
            //    Vector3 bounds = _textMeshPro.textBounds.size;
            //    _dialogBorder.size = new Vector2(bounds.x+0.3f, bounds.y + 0.5f);
            //}

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
