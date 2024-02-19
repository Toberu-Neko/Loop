using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SetCollider : CoreComponent
{
    private Movement movement;
    private CollisionSenses collisionSenses;
    private Stats stats;

    [SerializeField] private BoxCollider2D movementCollider;
    // [SerializeField] private float stuckColliderHeight = 0.5f;

    private bool changed;
    private float orgHeight;
    private float crouchHeight;
    private Vector2 v2Workspace;

    private bool changeable;
    public bool isCrouching;

    protected override void Awake()
    {
        base.Awake();

        collisionSenses = core.GetCoreComponent<CollisionSenses>();
        stats = core.GetCoreComponent<Stats>();
        movement = core.GetCoreComponent<Movement>();
    }

    private void OnEnable()
    {
        if (!movementCollider)
        {
            Debug.LogError("No collider set for " + core.transform.parent.name + " at " + this.name);
        }
        orgHeight = movementCollider.size.y;
        changed = false;
        changeable = true;
        isCrouching = false;

        movement.OnStuck += HandleOnStuck;
        stats.Health.OnCurrentValueZero += HandleHelthZero;
    }

    private void OnDisable()
    {
        movement.OnStuck -= HandleOnStuck;
        stats.Health.OnCurrentValueZero -= HandleHelthZero;
    }

    private void HandleHelthZero()
    {
        changeable = false;
    }

    private void HandleOnStuck()
    {
        if (!isCrouching)
        {
            if (!changed && changeable && collisionSenses.CanChangeCollider && !stats.IsTimeStopped)
            {
                changed = true;
                StartCoroutine(Change(0.75f));
            }
        }
        else
        {
            if (!changed && changeable && collisionSenses.CrouchCanChangeCollider && !stats.IsTimeStopped)
            {
                changed = true;
                crouchHeight = movementCollider.size.y;
                StartCoroutine(ChangeCrouchHeight(0.75f));
            }
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
        while(multiplier < 1f && changeable)
        {
            SetColliderHeight(orgHeight * multiplier);
            multiplier += 0.15f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SetColliderHeight(orgHeight);
    }

    private IEnumerator ChangeCrouchHeight(float multiplier)
    {
        while (multiplier < 1f && changeable)
        {
            SetColliderHeight(orgHeight * multiplier);
            multiplier += 0.15f;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (isCrouching)
        {
            SetColliderHeight(crouchHeight);
        }
        else
        {
            SetColliderHeight(orgHeight);
        }
    }

}
