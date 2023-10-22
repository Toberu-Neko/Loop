using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavepointUIChangeSkill : MonoBehaviour
{
    [SerializeField] private SavepointUIMain savepointUIMain;

    [Header("Weapon")] 
    [SerializeField] private Button slot1SwordButton;
    [SerializeField] private Button slot1GunButton;
    [SerializeField] private Button slot1FistButton;
    [SerializeField] private Button slot2SwordButton;
    [SerializeField] private Button slot2GunButton;
    [SerializeField] private Button slot2FistButton;

    [Header("TimeSkill")]
    [SerializeField] private PlayerTimeSkillManager playerTimeSkillManager;
    [SerializeField] private Button timeReverseButton;
    [SerializeField] private Button bookMarkButton;
    [SerializeField] private Button timeStopRangedButton;
    [SerializeField] private Button timeStopAllButton;
    [SerializeField] private Button bulletTimeAllButton;
    [SerializeField] private Button bulletTimeRangedButton;

    private TextMeshProUGUI slot1SwordText;
    private TextMeshProUGUI slot1GunText;
    private TextMeshProUGUI slot1FistText;
    private TextMeshProUGUI slot2SwordText;
    private TextMeshProUGUI slot2GunText;
    private TextMeshProUGUI slot2FistText;


    private void Awake()
    {
        slot1SwordText = slot1SwordButton.GetComponentInChildren<TextMeshProUGUI>();
        slot1GunText = slot1GunButton.GetComponentInChildren<TextMeshProUGUI>();
        slot1FistText = slot1FistButton.GetComponentInChildren<TextMeshProUGUI>();

        slot2SwordText = slot2SwordButton.GetComponentInChildren<TextMeshProUGUI>();
        slot2GunText = slot2GunButton.GetComponentInChildren<TextMeshProUGUI>();
        slot2FistText = slot2FistButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateMenu();
    }


    public void OnClickBackButton()
    {
        savepointUIMain.ActivateMenu();
        Deactivate();
    }

    public void OnClickSlot1Sword()
    {
        PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Sword);
        UpdateMenu();
    }
    public void OnClickSlot1Gun()
    {
        PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Gun);
        UpdateMenu();
    }
    public void OnClickSlot1Fist()
    {
        PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Fist);
        UpdateMenu();
    }

    public void OnClickSlot2Sword()
    {
        PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Sword);
        UpdateMenu();
    }

    public void OnClickSlot2Gun()
    {
        PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Gun);
        UpdateMenu();
    }

    public void OnClickSlot2Fist()
    {
        PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Fist);
        UpdateMenu();
    }

    private void ResetMenu()
    {
        slot1SwordButton.interactable = true;
        slot1GunButton.interactable = true;
        slot1FistButton.interactable = true;
        slot2SwordButton.interactable = true;
        slot2GunButton.interactable = true;
        slot2FistButton.interactable = true;

        slot1SwordText.color = Color.black;
        slot1GunText.color = Color.black;
        slot1FistText.color = Color.black;
        slot2SwordText.color = Color.black;
        slot2GunText.color = Color.black;
        slot2FistText.color = Color.black;

        timeReverseButton.interactable = true;
        bookMarkButton.interactable = true;
        timeStopRangedButton.interactable = true;
        timeStopAllButton.interactable = true;
        bulletTimeAllButton.interactable = true;
        bulletTimeRangedButton.interactable = true;
    }

    private void UpdateMenu()
    {
        ResetMenu();
        if (PlayerInventoryManager.Instance.EquipedWeapon[0] == WeaponType.Sword)
        {
            slot1SwordText.color = Color.red;
            slot1SwordButton.interactable = false;
            slot2SwordButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[0] == WeaponType.Gun)
        {
            slot1GunText.color = Color.red;
            slot1GunButton.interactable = false;
            slot2GunButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[0] == WeaponType.Fist)
        {
            slot1FistText.color = Color.red;
            slot1FistButton.interactable = false;
            slot2FistButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[1] == WeaponType.Sword)
        {
            slot2SwordText.color = Color.red;
            slot1SwordButton.interactable = false;
            slot2SwordButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[1] == WeaponType.Gun)
        {
            slot2GunText.color = Color.red;
            slot1GunButton.interactable = false;
            slot2GunButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[1] == WeaponType.Fist)
        {
            slot2FistText.color = Color.red;
            slot1FistButton.interactable = false;
            slot2FistButton.interactable = false;
        }

        if (!PlayerInventoryManager.Instance.CanUseFist)
        {
            slot1FistButton.interactable = false;
            slot2FistButton.interactable = false;
        }

        if (!PlayerInventoryManager.Instance.CanUseGun)
        {
            slot1GunButton.interactable = false;
            slot2GunButton.interactable = false;
        }

        if (!PlayerInventoryManager.Instance.CanUseSword)
        {
            slot1SwordButton.interactable = false;
            slot2SwordButton.interactable = false;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeSlowAll)
        {
            bulletTimeAllButton.interactable = false;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeSlowRanged)
        {
            bulletTimeRangedButton.interactable = false;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeStopAll)
        {
            timeStopAllButton.interactable = false;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeStopRanged)
        {
            timeStopRangedButton.interactable = false;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeReverse)
        {
            timeReverseButton.interactable = false;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.bookMark)
        {
            bookMarkButton.interactable = false;
        }
    }


    public void Activate()
    {
        gameObject.SetActive(true); 
        UpdateMenu();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        DataPersistenceManager.Instance.SaveGame();
    }
}
