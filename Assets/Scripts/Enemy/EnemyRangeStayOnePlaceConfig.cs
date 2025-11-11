using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Range Stay One Place Config", fileName = "EnemyRangeStayOnePlaceConfig")]
public class EnemyRangeStayOnePlaceConfig : ScriptableObject
{
    [Header("Typ przeciwnika")] public EnemyRangeStayOnePlaceScript.EnemyRangeType enemyRangeType;

    [Header("Typ Totemu (tylko dla Totem)")] [SerializeField]
    private EnemyRangeStayOnePlaceScript.TotemType totemType;

    [SerializeField] private EnemyRangeStayOnePlaceScript.PartType partType;

    [Header("Animator")] public AnimatorOverrideController animatorController;

    [Header("Strzelanie")] 
    public bool isAutoAttack = true;
    public EnemyRangeStayOnePlaceBulletConfig projectileConfig; // zamiast prefab
    public GameObject projectilePrefab;   // pozostaw prefab tylko do instancjacji


    [Header("Parametry")] public int startHP = 5;

    [Header("Nazwy stanów animacji")] public string dieStateName = "Die";

    [Header("Dopasowanie collidera")] public ColliderAdjust colliderAdjust;
    public EnemyRangeStayOnePlaceScript.TotemType? TotemType => 
        enemyRangeType == EnemyRangeStayOnePlaceScript.EnemyRangeType.Totem ? totemType : null;

    public EnemyRangeStayOnePlaceScript.PartType? PartType => 
        enemyRangeType == EnemyRangeStayOnePlaceScript.EnemyRangeType.Totem ? partType : null;


    [System.Serializable]
    public struct ColliderAdjust
    {
        [Range(0f, 0.5f)] public float trimLeftPercent;
        [Range(0f, 0.5f)] public float trimRightPercent;
        [Range(0f, 0.5f)] public float trimTopPercent;
        [Range(-0.2f, 0.5f)] public float trimBottomPercent;
    }

}