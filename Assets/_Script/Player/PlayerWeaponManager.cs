using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerWeaponType CurrentWeaponType { get; private set; }
    public int SwordEnergy { get; private set; }

    private void Start()
    {
        CurrentWeaponType = PlayerWeaponType.Sword;
        SwordEnergy = 0;
    }
}
public enum PlayerWeaponType
{
    Sword,
    Axe,
    Gun
}
