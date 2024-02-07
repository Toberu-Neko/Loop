using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.UI;

public class HUDStatus : MonoBehaviour
{
    // Bar Controls by player debug component
    [Header("Player")]
    [SerializeField] private DebugPlayerComp playerEventHandler;
    [SerializeField] private HealthBar playerHealthBar;
    [SerializeField] private HealthBar timeEnergyBar;
    [SerializeField] private HealthBar gunEnergyBar;

    [SerializeField] private GameObject timeUIObj;
    [Header("Weapon")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Sprite sword;
    [SerializeField] private Sprite gun;
    [SerializeField] private Sprite fist;

    [SerializeField] private GameObject weaponUIMotherObj;
    [SerializeField] private GameObject weaponEnergy1;
    [SerializeField] private GameObject weaponEnergy2;
    [SerializeField] private GameObject weaponEnergy3;


    [SerializeField] private LocalizeStringEvent timeSkillText;
    [SerializeField] private LocalizedString defaultLocalizedString;
    private void Awake()
    {
    }

    private void OnEnable()
    {
        playerEventHandler.OnInit += InitBars;
        playerEventHandler.OnUpdateHp += playerHealthBar.UpdateHealthBar;
        playerEventHandler.OnUpdateTimeSkill += UpdateTimeSkill;
        playerEventHandler.OnUpdateWeapon += UpdateWeapon;
    }

    private void OnDisable()
    {
        playerEventHandler.OnInit -= InitBars;
        playerEventHandler.OnUpdateHp -= playerHealthBar.UpdateHealthBar;
        playerEventHandler.OnUpdateTimeSkill -= UpdateTimeSkill;
        playerEventHandler.OnUpdateWeapon -= UpdateWeapon;
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    private void InitBars(float hp, float time, float gun)
    {
        playerHealthBar.Init(hp);
        timeEnergyBar.Init(time);
        gunEnergyBar.Init(gun);
    }

    private void UpdateTimeSkill(LocalizedString skillName, float energy, bool active = true)
    {
        if(!active)
        {
            timeUIObj.SetActive(false);
            return;
        }

        if(!timeUIObj.activeInHierarchy)
            timeUIObj.SetActive(true);

        timeSkillText.StringReference = skillName;
        timeEnergyBar.UpdateHealthBar(energy);
    }

    private void UpdateWeapon(WeaponType type, int energy, float gunNormalEnergy)
    {
        switch (type)
        {
            case WeaponType.Sword:
                weaponIcon.sprite = sword;
                weaponUIMotherObj.SetActive(true);
                gunEnergyBar.gameObject.SetActive(false);
                UpdateWeaponEnergy(energy);
                break;
            case WeaponType.Gun:
                weaponIcon.sprite = gun;
                UpdateWeaponEnergy(energy);
                weaponUIMotherObj.SetActive(true);
                gunEnergyBar.gameObject.SetActive(true);
                gunEnergyBar.UpdateHealthBar(gunNormalEnergy);
                break;
            case WeaponType.Fist:
                weaponIcon.sprite = fist;
                weaponUIMotherObj.SetActive(true);
                gunEnergyBar.gameObject.SetActive(false);
                UpdateWeaponEnergy(energy);
                break;

            case WeaponType.None:
                weaponIcon.sprite = null;
                weaponUIMotherObj.SetActive(false);
                break;
        }
    }

    private void UpdateWeaponEnergy(int energy)
    {
        switch (energy)
        {
            case 0:
                weaponEnergy1.SetActive(false);
                weaponEnergy2.SetActive(false);
                weaponEnergy3.SetActive(false);
                break;
            case 1:
                weaponEnergy1.SetActive(true);
                weaponEnergy2.SetActive(false);
                weaponEnergy3.SetActive(false);
                break;
            case 2:
                weaponEnergy1.SetActive(true);
                weaponEnergy2.SetActive(true);
                weaponEnergy3.SetActive(false);
                break;
            case 3:
                weaponEnergy1.SetActive(true);
                weaponEnergy2.SetActive(true);
                weaponEnergy3.SetActive(true);
                break;
            default:
                weaponEnergy1.SetActive(false);
                weaponEnergy2.SetActive(true);
                weaponEnergy3.SetActive(false);
                Debug.LogError("Weapon Energy More Then 3");
                break;
        }
    }

}
