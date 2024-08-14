/// <summary>
/// This class is used to give the player a time skill item when the boss dies.
/// </summary>
public class GiveTimeSkillDead : CoreComponent
{
    private Death death;

    private SO_TimeSkillItem timeSkillItem;

    protected override void Awake()
    {
        base.Awake();

        death = core.GetCoreComponent<Death>();
        timeSkillItem = core.CoreData.timeSkillItem;
    }

    private void OnEnable()
    {
        death.OnDeath += HandleDead;
    }

    private void OnDisable()
    {
        death.OnDeath -= HandleDead;
    }

    private void HandleDead()
    {
        if (timeSkillItem == null)
            return;


        PlayerInventoryManager.Instance.AddTimeSkillItem(timeSkillItem.ID);
        UI_Manager.Instance.ActivePickupItemUI(timeSkillItem.displayNameLocalization, timeSkillItem.shortDescriptionLocalization);
        UI_Manager.Instance.ResetAllInput();

        if(timeSkillItem.popupTutorialLocalization!= null)
        {
            if (timeSkillItem.popupTutorialLocalization.Length > 0)
            {
                foreach (var item in timeSkillItem.popupTutorialLocalization)
                {
                    UI_Manager.Instance.ActivateTutorialPopUpUI(item);
                }
            }
        }
    }
}
