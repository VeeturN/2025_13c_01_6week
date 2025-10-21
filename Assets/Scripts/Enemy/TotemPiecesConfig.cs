using UnityEngine;

[CreateAssetMenu(fileName = "TotemPiecesConfig", menuName = "Configs/Totem Pieces Config")]
public class TotemPiecesConfig : ScriptableObject
{
    [System.Serializable]
    public class TotemPieceVariant
    {
        public TotemScript.TotemType totemType;
        public TotemScript.PartType partType;
        public Sprite[] pieceSprites;   // domyslnie 3
    }

    public TotemPieceVariant[] variants;

    // zwraca tutaj dobre sprity
    public Sprite[] GetSprites(TotemScript.TotemType totemType, TotemScript.PartType partType)
    {
        foreach (var v in variants)
        {
            if (v.totemType == totemType && v.partType == partType)
                return v.pieceSprites;
        }
        return null;
    }
}