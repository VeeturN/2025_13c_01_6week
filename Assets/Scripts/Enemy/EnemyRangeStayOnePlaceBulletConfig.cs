using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Bullet Config", fileName = "BulletConfig")]
public class EnemyRangeStayOnePlaceBulletConfig : ScriptableObject
{
    [Header("Parametry lotu")]
    public float speed = 5f;
    public float lifetime = 3f;
    public int damage = 1;

    [Header("Animator / typ pocisku")]
    public AnimatorOverrideController animatorController;

    [Header("Dopasowanie collidera")]
    public ColliderAdjust colliderAdjust;
    [Header("Pozycja pocisku (offset od punktu startu)")]
    public Vector2 spawnOffset = Vector2.zero;


    [System.Serializable]
    public struct ColliderAdjust
    {
        [Range(0f, 0.5f)] public float trimLeftPercent;
        [Range(0f, 0.5f)] public float trimRightPercent;
        [Range(0f, 0.5f)] public float trimTopPercent;
        [Range(0f, 0.5f)] public float trimBottomPercent;
    }
}