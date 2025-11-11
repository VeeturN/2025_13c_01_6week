using TMPro;
using UnityEngine;

public class CapslockOnlyText : MonoBehaviour
{
    private TMP_InputField input;

    void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }

    void Start()
    {
        input.onValidateInput += ForceUppercase;
    }

    private char ForceUppercase(string text, int index, char addedChar)
    {
        return char.ToUpper(addedChar);
    }
}
