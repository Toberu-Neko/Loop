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
    [SerializeField] private Button noneButton;
    [SerializeField] private GameObject firstSelectedObj;

    private TextMeshProUGUI slot1SwordText;
    private TextMeshProUGUI slot1GunText;
    private TextMeshProUGUI slot1FistText;
    private TextMeshProUGUI slot2SwordText;
    private TextMeshProUGUI slot2GunText;
    private TextMeshProUGUI slot2FistText;

    private TextMeshProUGUI timeReverseText;
    private TextMeshProUGUI bookMarkText;
    private TextMeshProUGUI timeStopRangedText;
    private TextMeshProUGUI timeStopAllText;
    private TextMeshProUGUI bulletTimeAllText;
    private TextMeshProUGUI bulletTimeRangedText;
    private TextMeshProUGUI noneText;


    private void Awake()
    {
        slot1SwordText = slot1SwordButton.GetComponentInChildren<TextMeshProUGUI>();
        slot1GunText = slot1GunButton.GetComponentInChildren<TextMeshProUGUI>();
        slot1FistText = slot1FistButton.GetComponentInChildren<TextMeshProUGUI>();

        slot2SwordText = slot2SwordButton.GetComponentInChildren<TextMeshProUGUI>();
        slot2GunText = slot2GunButton.GetComponentInChildren<TextMeshProUGUI>();
        slot2FistText = slot2FistButton.GetComponentInChildren<TextMeshProUGUI>();

        timeReverseText = timeReverseButton.GetComponentInChildren<TextMeshProUGUI>();
        bookMarkText = bookMarkButton.GetComponentInChildren<TextMeshProUGUI>();
        timeStopRangedText = timeStopRangedButton.GetComponentInChildren<TextMeshProUGUI>();
        timeStopAllText = timeStopAllButton.GetComponentInChildren<TextMeshProUGUI>();
        bulletTimeAllText = bulletTimeAllButton.GetComponentInChildren<TextMeshProUGUI>();
        bulletTimeRangedText = bulletTimeRangedButton.GetComponentInChildren<TextMeshProUGUI>();
        noneText = noneButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateMenu();
    }


    public void OnClickBackButton()
    {
        Deactivate();
        savepointUIMain.ActivateMenu();
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

        slot1SwordText.color = Color.white;
        slot1GunText.color = Color.white;
        slot1FistText.color = Color.white;
        slot2SwordText.color = Color.white;
        slot2GunText.color = Color.white;
        slot2FistText.color = Color.white;

        timeReverseButton.interactable = true;
        bookMarkButton.interactable = true;
        timeStopRangedButton.interactable = true;
        timeStopAllButton.interactable = true;
        bulletTimeAllButton.interactable = true;
        bulletTimeRangedButton.interactable = true;
    }

    public void UpdateMenu()
    {
        ResetMenu();
        if (PlayerInventoryManager.Instance.EquipedWeapon[0] == WeaponType.Sword)
        {
            slot1SwordText.color = Color.red;
            slot2SwordText.color = Color.gray;
            slot1SwordButton.interactable = false;
            slot2SwordButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[0] == WeaponType.Gun)
        {
            slot1GunText.color = Color.red;
            slot2GunText.color = Color.gray;
            slot1GunButton.interactable = false;
            slot2GunButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[0] == WeaponType.Fist)
        {
            slot1FistText.color = Color.red;
            slot2FistText.color = Color.gray;
            slot1FistButton.interactable = false;
            slot2FistButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[1] == WeaponType.Sword)
        {
            slot1SwordText.color = Color.gray;
            slot2SwordText.color = Color.red;
            slot1SwordButton.interactable = false;
            slot2SwordButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[1] == WeaponType.Gun)
        {
            slot1GunText.color = Color.gray;
            slot2GunText.color = Color.red;
            slot1GunButton.interactable = false;
            slot2GunButton.interactable = false;
        }

        if (PlayerInventoryManager.Instance.EquipedWeapon[1] == WeaponType.Fist)
        {
            slot1FistText.color = Color.gray;
            slot2FistText.color = Color.red;
            slot1FistButton.interactable = false;
            slot2FistButton.interactable = false;
        }

        if (!PlayerInventoryManager.Instance.CanUseFist)
        {
            slot2FistText.color = Color.gray;
            slot1FistText.color = Color.gray;
            slot1FistButton.interactable = false;
            slot2FistButton.interactable = false;
        }

        if (!PlayerInventoryManager.Instance.CanUseGun)
        {
            slot1GunText.color = Color.gray;
            slot2GunText.color = Color.gray;
            slot1GunButton.interactable = false;
            slot2GunButton.interactable = false;
        }

        if (!PlayerInventoryManager.Instance.CanUseSword)
        {
            slot1SwordText.color = Color.gray;
            slot2SwordText.color = Color.gray;
            slot1SwordButton.interactable = false;
            slot2SwordButton.interactable = false;
        }

        if(playerTimeSkillManager.StateMachine.CurrentState == playerTimeSkillManager.SkillNone)
        {
            noneButton.interactable = false;
        }

        timeReverseButton.interactable = true;
        bookMarkButton.interactable = true;
        timeStopRangedButton.interactable = true;
        timeStopAllButton.interactable = true;
        bulletTimeAllButton.interactable = true;
        bulletTimeRangedButton.interactable = true;

        timeReverseText.color = Color.white;
        bookMarkText.color = Color.white;
        timeStopRangedText.color = Color.white;
        timeStopAllText.color = Color.white;
        bulletTimeAllText.color = Color.white;
        bulletTimeRangedText.color = Color.white;


        if (playerTimeSkillManager.StateMachine.CurrentState == playerTimeSkillManager.SkillRewindPlayer)
        {
            timeReverseButton.interactable = false;
            timeReverseText.color = Color.red;
        }

        if(playerTimeSkillManager.StateMachine.CurrentState == playerTimeSkillManager.SkillBookMark)
        {
            bookMarkButton.interactable = false;
            bookMarkText.color = Color.red;
        }

        if(playerTimeSkillManager.StateMachine.CurrentState == playerTimeSkillManager.SkillTimeStopThrow)
        {
            timeStopRangedButton.interactable = false;
            timeStopRangedText.color = Color.red;
        }

        if(playerTimeSkillManager.StateMachine.CurrentState == playerTimeSkillManager.SkillTimeStopAll)
        {
            timeStopAllButton.interactable = false;
            timeStopAllText.color = Color.red;
        }

        if(playerTimeSkillManager.StateMachine.CurrentState == playerTimeSkillManager.SkillBulletTimeAll)
        {
            bulletTimeAllButton.interactable = false;
            bulletTimeAllText.color = Color.red;
        }

        if(playerTimeSkillManager.StateMachine.CurrentState == playerTimeSkillManager.SkillBulletTimeRanged)
        {
            bulletTimeRangedButton.interactable = false;
            bulletTimeRangedText.color = Color.red;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeSlowAll)
        {
            bulletTimeAllButton.interactable = false;
            bulletTimeAllText.color = Color.gray;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeSlowRanged)
        {
            bulletTimeRangedButton.interactable = false;
            bulletTimeRangedText.color = Color.gray;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeStopAll)
        {
            timeStopAllButton.interactable = false;
            timeStopAllText.color = Color.gray;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeStopRanged)
        {
            timeStopRangedButton.interactable = false;
            timeStopRangedText.color = Color.gray;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.timeReverse)
        {
            timeReverseButton.interactable = false;
            timeReverseText.color = Color.gray;
        }

        if(!playerTimeSkillManager.UnlockedTimeSkills.bookMark)
        {
            bookMarkButton.interactable = false;
            bookMarkText.color = Color.gray;
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
