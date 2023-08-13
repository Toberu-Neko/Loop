using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCoreData", menuName = "Data/Core Data/Core Data")]
public class CoreData : ScriptableObject
{
    [Header("Stats")]
    [Tooltip("最大生命值")]
    public float maxHealth = 150f;
    [Tooltip("最大耐力值")]
    public float maxStamina = 30f;
    [Tooltip("耐力回復效率（每秒）")]
    public float staminaRecoveryRate = 5f;
    [Tooltip("被打到後的無敵時間")]
    public float invincibleDurationAfterDamaged = 0.2f;
    [Tooltip("被打到後，進入戰鬥狀態的時間")]
    public float combatTimer = 5f;

    [Header("如果物件不會完美防禦，不用填")]
    [Tooltip("完美防禦後的反擊攻擊持續時間")]
    public float perfectBlockAttackDuration = 2f;


    [Space(10)]
    [Header("Combat")]
    [Tooltip("被打到時會生成的粒子效果")]
    public GameObject damageParticles;
    [Tooltip("最大擊退持續時間")]
    public float maxKnockbackTime = 0.3f;
    [Header("如果物件不會防禦，不用填")]
    [Tooltip("普通防禦狀態下的傷害減免，傷害 = 原始傷害 * 數值")]
    public float blockDamageMultiplier = 0.5f;
    [Tooltip("普通防禦狀態下的耐力減免，傷害 = 原始傷害 * 數值")]
    public float blockStaminaMultiplier = 0.5f;
    [Tooltip("普通防禦狀態下的擊退向量。")]
    public Vector2 normalBlockKnockbakDirection = new(1, 0.25f);
    [Tooltip("普通防禦狀態下的擊退減免，擊退 = 原始擊退力量 * 數值")]
    public float normalBlockKnockbakMultiplier = 0.75f;

    [Space(10)]
    [Header("CollisionSenese")] 
    public PhysicsMaterial2D noFrictionMaterial;
    public PhysicsMaterial2D fullFrictionMaterial;

    [Space(10)]
    [Header("Death")]
    [Tooltip("死掉的時候會生成的粒子效果")]
    public GameObject[] deathParticles;
    [Header("Loot, 如果物件不會掉東西, 不用填")]
    public GameObject dropItemPrefab;
    public List<LootItem> lootItems;
}
