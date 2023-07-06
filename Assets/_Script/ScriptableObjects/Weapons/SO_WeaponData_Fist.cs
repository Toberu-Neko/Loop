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

    [Header("Strong Attack")]
    [Tooltip("Ĳ�o�Ĥ@���W�O�����һݪ��ɶ�")]
    public float strongAttackHoldTime;
    [Tooltip("�C���R�ఫ��һݪ��ɶ�")]
    public float everySoulAddtionalHoldTime;
    public WeaponAttackDetails[] soulAttackDetails;

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
