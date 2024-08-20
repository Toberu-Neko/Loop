using UnityEngine;

public class ChargeState : EnemyState
{
    protected ED_EnemyChargeState stateData;

    private Vector2 lastAfterImagePosition;
    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool gotoNextState;
    protected bool performCloseRangeAction;
    public ChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront || CollisionSenses.WallFrontHead;

        performCloseRangeAction = CheckPlayerSenses.IsPlayerInCloseRangeAction && !isChargeTimeOver;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
        isChargeTimeOver = false;
        gotoNextState = false;

        entity.SetSkillCollideDamage(true);
    }

    public override void Exit()
    {
        base.Exit();

        if(stateData.thingsToSay != null)
        {
            if (stateData.thingsToSay.Length > 0 && !saidThings)
            {
                foreach (var thing in stateData.thingsToSay)
                {
                    UI_Manager.Instance.ActivateTutorialPopUpUI(thing);
                }

                saidThings = true;
            }
        }

        entity.SetSkillCollideDamage(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isChargeTimeOver && !isDetectingWall)
        {
            CheckShouldPlaceAfterImage();

            if (Stats.IsAngry)
            {
                Movement.SetVelocityX(stateData.angryChargeSpeed * Movement.FacingDirection);
            }
            else
            {
                Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
            }
        }
        else
        {
            Movement.SetVelocityX(0f);
        }

        if (Time.time >= StartTime + stateData.chargeTime && !isChargeTimeOver)
        {
            isChargeTimeOver = true;
        }

        if(Time.time >= StartTime + stateData.chargeTime + stateData.finishChargeDelay && !gotoNextState)
        {
            gotoNextState = true;
        }
    }

    private void CheckShouldPlaceAfterImage()
    {
        if (Vector2.Distance(Movement.ParentTransform.position, lastAfterImagePosition) >= stateData.afterImageDistance && !(Stats.IsTimeSlowed || Stats.IsTimeStopped))
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        var obj = ObjectPoolManager.SpawnObject(stateData.afterImagePrefab, Movement.ParentTransform.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sprite = entity.spriteRenderer.sprite;
        obj.transform.localScale = Movement.ParentTransform.localScale;
        obj.transform.rotation = Movement.ParentTransform.rotation;

        lastAfterImagePosition = Movement.ParentTransform.position;
    }

    public bool CheckCanCharge()
    {
        return Time.time > EndTime + stateData.chargeCooldown;
    }
}
