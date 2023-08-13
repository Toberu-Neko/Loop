using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCoreData", menuName = "Data/Core Data/Core Data")]
public class CoreData : ScriptableObject
{
    [Header("Stats")]
    [Tooltip("�̤j�ͩR��")]
    public float maxHealth = 150f;
    [Tooltip("�̤j�@�O��")]
    public float maxStamina = 30f;
    [Tooltip("�@�O�^�_�Ĳv�]�C��^")]
    public float staminaRecoveryRate = 5f;
    [Tooltip("�Q����᪺�L�Įɶ�")]
    public float invincibleDurationAfterDamaged = 0.2f;
    [Tooltip("�Q�����A�i�J�԰����A���ɶ�")]
    public float combatTimer = 5f;

    [Header("�p�G���󤣷|�������m�A���ζ�")]
    [Tooltip("�������m�᪺������������ɶ�")]
    public float perfectBlockAttackDuration = 2f;


    [Space(10)]
    [Header("Combat")]
    [Tooltip("�Q����ɷ|�ͦ����ɤl�ĪG")]
    public GameObject damageParticles;
    [Tooltip("�̤j���h����ɶ�")]
    public float maxKnockbackTime = 0.3f;
    [Header("�p�G���󤣷|���m�A���ζ�")]
    [Tooltip("���q���m���A�U���ˮ`��K�A�ˮ` = ��l�ˮ` * �ƭ�")]
    public float blockDamageMultiplier = 0.5f;
    [Tooltip("���q���m���A�U���@�O��K�A�ˮ` = ��l�ˮ` * �ƭ�")]
    public float blockStaminaMultiplier = 0.5f;
    [Tooltip("���q���m���A�U�����h�V�q�C")]
    public Vector2 normalBlockKnockbakDirection = new(1, 0.25f);
    [Tooltip("���q���m���A�U�����h��K�A���h = ��l���h�O�q * �ƭ�")]
    public float normalBlockKnockbakMultiplier = 0.75f;

    [Space(10)]
    [Header("CollisionSenese")] 
    public PhysicsMaterial2D noFrictionMaterial;
    public PhysicsMaterial2D fullFrictionMaterial;

    [Space(10)]
    [Header("Death")]
    [Tooltip("�������ɭԷ|�ͦ����ɤl�ĪG")]
    public GameObject[] deathParticles;
    [Header("Loot, �p�G���󤣷|���F��, ���ζ�")]
    public GameObject dropItemPrefab;
    public List<LootItem> lootItems;
}
