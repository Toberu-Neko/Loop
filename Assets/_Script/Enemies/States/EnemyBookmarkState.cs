using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBookmarkState : EnemyState
{
    protected S_EnemyBookmarkState stateData;

    public Vector2 bookmarkPosition;
    private GameObject bookmarkObj;
    public bool isBookmarkActive { get; private set; }
    public EnemyBookmarkState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyBookmarkState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        bookmarkPosition = Vector2.zero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
        else
        {
            Movement.SetVelocityX(0f);
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        bookmarkObj = ObjectPoolManager.SpawnObject(stateData.bookmarkPrefab, entity.GetPosition(), Quaternion.identity);
        SpriteRenderer sr = bookmarkObj.GetComponent<SpriteRenderer>();
        sr.sprite = entity.GetCurrentSprite();
        bookmarkPosition = entity.GetPosition();
        isBookmarkActive = true;
    }

    public Vector2 GetBookmarkPosition()
    {
        return bookmarkPosition;
    }

    public void ResetBookmark()
    {
        bookmarkPosition = Vector2.zero;
        isBookmarkActive = false;
        ObjectPoolManager.ReturnObjectToPool(bookmarkObj);
    }

}
