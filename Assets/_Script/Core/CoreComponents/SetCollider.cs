using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCollider : CoreComponent
{
    private Movement movement;
    private CollisionSenses collisionSenses;

    [SerializeField] private BoxCollider2D movementCollider;
    // [SerializeField] private float stuckColliderHeight = 0.5f;

    private bool changed;
    private float orgHeight;
    private Vector2 v2Workspace;

    protected override void Awake()
    {
        base.Awake();

        collisionSenses = core.GetCoreComponent<CollisionSenses>();

        movement = core.GetCoreComponent<Movement>();
        movement.OnStuck += HandleOnStuck;

        orgHeight = movementCollider.size.y;
        changed = false;
    }
    private void OnDisable()
    {
        movement.OnStuck -= HandleOnStuck;
    }

    private void HandleOnStuck()
    {
        if (!changed && collisionSenses.CanChangeCollider)
        {
            changed = true;
            StartCoroutine(Change(0.65f));
        }
    }

    public void SetColliderHeight(float height)
    {
        v2Workspace.Set(movementCollider.size.x, height);


        movementCollider.size = v2Workspace;

        if(height == orgHeight)
        {
            changed = false;
        }
    }

    private IEnumerator Change(float multiplier)
    {
        while(multiplier < 1f)
        {
            SetColliderHeight(orgHeight * multiplier);
            multiplier += 0.15f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SetColliderHeight(orgHeight);
    }

}
