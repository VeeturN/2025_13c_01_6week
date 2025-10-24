using UnityEngine;
using static System.StringComparison;

[CreateAssetMenu(fileName = "ObjectDestructionConfig", menuName = "Configs/Object Destruction Config")]
public class ObjectDestructionConfig : ScriptableObject
{
    [System.Serializable]
    public class ObjectVariant
    {
        [Header("Identyfikator wariantu (np. Totem, Armata, Malza, Beczka)")]
        public string objectType;

        [Header("Część obiektu (np. Body, Head, Base) - użyj '*' aby pasowało wszystko")]
        public string partType;

        [Header("Sprite'y kawałków po zniszczeniu")]
        public Sprite[] pieceSprites;
    }

    [Header("Warianty zniszczenia obiektów")]
    public ObjectVariant[] variants;
    
    public Sprite[] GetSprites(string objectType, string partType)
    {
        foreach (var v in variants)
        {
            bool objectMatch = v.objectType.Equals(objectType, OrdinalIgnoreCase);
            bool partMatch = string.IsNullOrEmpty(v.partType) || v.partType == "*" 
                                                              || v.partType.Equals(partType, OrdinalIgnoreCase);

            if (objectMatch && partMatch)
                return v.pieceSprites;
        }
        Debug.LogWarning($"[ObjectDestructionConfig] Brak sprite'ów dla {objectType}/{partType}");
        return null;
    }
}