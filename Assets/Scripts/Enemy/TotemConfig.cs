
using UnityEngine;
using Enemy;


[CreateAssetMenu(menuName = "Configs/Totem Config",fileName = "TotemConfig")]
public class TotemConfig : ScriptableObject
{ 
        [Header("Typ Totemu")]
        public TotemScript.TotemType totemType;
        public TotemScript.PartType partType;

        [Header("Animator Totemu")]
        public AnimatorOverrideController animatorController;

        [Header("Strzelanie Totemu")]
        public bool isAutoAttack = true;
        public GameObject projectilePrefab;

        [Header("Parametry Totemu")]
        public int startHP = 5;

        [Header("Nazwy stanów animacji")]
        public string dieStateName = "Die";
        [Header("Dopasowanie collidera")]
        public ColliderAdjust colliderAdjust;
        //dopasowanie coliderów
        [System.Serializable]
        public struct ColliderAdjust
        {
                [Range(0f, 0.5f)] public float trimLeftPercent;
                [Range(0f, 0.5f)] public float trimRightPercent;
                [Range(0f, 0.5f)] public float trimTopPercent;
                [Range(0f, 0.5f)] public float trimBottomPercent;
        }
}

