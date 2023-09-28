using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FistData", menuName = "Data/WeaponData/FistData")]
public class SO_WeaponData_Fist : SO_WeaponData
{
    [Tooltip("�̤j��q")]
    public int maxEnergy;

    [Tooltip("����Z�������p�ƾ��һݪ��D�����ɶ�")]
    [Header("Normal Attack")]
    public float resetAttackTime;
    [SerializeField] private WeaponAttackDetails[] normalAttackDetails;

    [Header("Counter Attack")]
    public WeaponAttackDetails counterAttackDetails;

    [Header("Strong Attack")]
    [Tooltip("�W�O�����һݪ���O�ɶ�")]
    public float strongAttackHoldTime;
    public WeaponAttackDetails strongAttackDetail;
    public WeaponAttackDetails soulAttackDetail;

    [Header("Static Strong Attack")]
    public WeaponAttackDetails staticStrongAttackDetail;
    public WeaponAttackDetails staticSoulAttackDetail;

    [Header("Soul Max Attack")]
    public float s3ChargeSpeed;
    public float s3MaxChargeTime;
    public float s3GoUpVelocity;
    public WeaponAttackDetails[] soulThreeAttackDetails;

    public WeaponAttackDetails[] NormalAttackDetails { get => normalAttackDetails; private set => normalAttackDetails = value; }
    private void OnEnable()
    {
        AmountOfAttacks = normalAttackDetails.Length;

        MovementSpeed = new float[AmountOfAttacks];

        for (int i = 0; i < AmountOfAttacks; i++)
        {
            MovementSpeed[i] = normalAttackDetails[i].movementSpeed;
        }
    }
}
