using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour, IDamageable
{
    private enum State
    {
        Moving,
        Knockback,
        Dead
    }
    private State currentState;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float knockbackDuration;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    [SerializeField] private Vector2 knockbackSpeed;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private GameObject hitParticle;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject deathBloodParticle;
    private int facingDirection;
    private int damageDirection;

    private float currentHealth;
    private float knockbackStartTime;

    private Vector2 movement;

    private bool groundDetected;
    private bool wallDetected;

    private GameObject alive;
    private Rigidbody2D aliveRB;
    private Animator aliveAnim;

    private void Awake()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRB = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();

        facingDirection = 1;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        switch (currentState)
        {
            case State.Moving:
                UpdateWalkingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }
    //--Walking----------------------------------------------------------------------------------
    private void EnterWalkingState()
    {

    }
    private void UpdateWalkingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right * facingDirection, wallCheckDistance, whatIsGround);

        // Debug.Log(wallDetected);

        if(!groundDetected || wallDetected)
        {
            Flip();
        }
        else
        {
            movement.Set(movementSpeed * facingDirection, aliveRB.velocity.y);
            aliveRB.velocity = movement;
        }
    }
    private void ExitWalkingState()
    {

    }
    //--Knockback----------------------------------------------------------------------------------
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRB.velocity = movement;

        aliveAnim.SetBool("knockback", true);
    }
    private void UpdateKnockbackState()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }
    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("knockback", false);
    }
    //--Dead----------------------------------------------------------------------------------
    private void EnterDeadState()
    {
        Instantiate(deathParticle, alive.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        Instantiate(deathBloodParticle, alive.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        Destroy(gameObject);
    }
    private void UpdateDeadState()
    {

    }
    private void ExitDeadState()
    {

    }

    #region Other Functions
    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0f, 180f, 0f);
    }
    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Moving:
                ExitWalkingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }
        switch (state)
        {
            case State.Moving:
                EnterWalkingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
        currentState = state;
    }

    public void Damage(AttackDetails details)
    {
        currentHealth -= details.damageAmount;

        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if(details.position.x > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        if(currentHealth > 0f)
        {
            SwitchState(State.Knockback);
        }
        else if(currentHealth <= 0f)
        {
            SwitchState(State.Dead);
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
