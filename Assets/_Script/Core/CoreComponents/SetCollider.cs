using System.Collections;
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
    private Vector2 v2Workspace;

    protected override void Awake()
    {
        base.Awake();

        collisionSenses = core.GetCoreComponent<CollisionSenses>();
        stats = core.GetCoreComponent<Stats>();

        movement = core.GetCoreComponent<Movement>();

    }

    private void OnEnable()
    {
        orgHeight = movementCollider.size.y;
        changed = false;

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
        this.enabled = false;
    }

    private void HandleOnStuck()
    {
        if (!changed && collisionSenses.CanChangeCollider && !stats.IsTimeStopped)
        {
            changed = true;
            StartCoroutine(Change(0.75f));
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
