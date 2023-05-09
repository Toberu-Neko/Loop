using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool m_AirControl = true;                          // Whether or not a player can steer while jumping;
	[SerializeField] private bool onWallGravity;
	[SerializeField] private float maxAngle;
	
	[Header("Jump")]
    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_GroundLayer;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_WallCheck;                             //Posicion que controla si el personaje toca una pared
	
	[Header("WallJump")]
	[SerializeField] private float wallJumpForceX;
	[SerializeField] private float endSlidingDelay;
    [Header("Dash")]
	[SerializeField] private float dashDuration;
	[SerializeField] private float dashCooldown;
	[SerializeField] private float m_DashForce = 25f;
	
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 25f; // Limit fall speed

	public bool canDoubleJump = true; //If player can double jump
	private bool canDash = true;
	private bool isDashing = false; //If player is dashing
	private bool m_WallInfront = false; //If there is a wall in front of the player
	private bool isWallSliding = false; //If player is sliding in a wall
	private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
	private float prevVelocityX = 0f;
	private bool canCheck = false; //For check if player is wallsliding
	private bool canMove = true; //If player can move

	private Animator animator;
	public ParticleSystem particleJumpUp; //Trail particles
	public ParticleSystem particleJumpDown; //Explosion particles

	private float jumpWallStartX = 0;
	private float jumpWallDistX = 0; //Distance between player and wall
	private bool limitVelOnWallJump = false; //For limit wall jump distance with low fps

	private PlayerBattle playerBattle;

	[HideInInspector] public bool turnAble;
    private void Awake()
	{
		turnAble = true;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
        playerBattle = GetComponent<PlayerBattle>();
	}


	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		// Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_GroundLayer);
        RaycastHit2D groundHit = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, 1f, m_GroundLayer);
		if(groundHit.collider != null)
		{
            Vector2 normal = groundHit.normal;
            float angle = Mathf.Atan2(normal.x, normal.y) * Mathf.Rad2Deg;
            //Debug.Log("¦aªO¨¤«×¬°¡G" + angle);
			if (angle > maxAngle || angle < -maxAngle)
			{	
                m_Rigidbody2D.AddForce(new(0, -100f));
            }

            else if (groundHit.distance < 0.2f)
			{
                m_Grounded = true;
                if (!wasGrounded)
                {
                    // OnLandEvent.Invoke();
                    animator.SetBool("Grounded", m_Grounded);

                    if (!m_WallInfront && !isDashing)               //If player is not beside a wall or dashing
                        canDoubleJump = true;

                    if (m_Rigidbody2D.velocity.y < 0f)
                        limitVelOnWallJump = false;
                }
            }
        }

        m_WallInfront = false;

		if (!m_Grounded)
		{
            animator.SetBool("Grounded", m_Grounded);

			// Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_GroundLayer);
			Vector2 hitDir = transform.right;

            if (isWallSliding)
				hitDir = transform.right * -1;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, hitDir, 3, m_GroundLayer);
            if (hit.collider != null)
            {
                Vector2 normal = hit.normal;
                float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
                // print("Angle: " + angle);
				// print("Distance: " + hit.distance);

				hit.distance = Mathf.Abs(hit.distance);
				if(hit.distance < 0.35f && 
					((angle < 182 && angle > 178) 
					|| (angle < 2 && angle > -2)
					|| (angle > -182 && angle < -178)))
				{
					isDashing = false;
					m_WallInfront = true;
				}
            }
            
			/*
            for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != null && collidersWall[i] is BoxCollider2D)
				{
					isDashing = false;
					m_WallInfront = true;

                    BoxCollider2D boxCollider = (BoxCollider2D)collidersWall[i];
                    Vector2 bottomLeft = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y);
                    Vector2 bottomRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y);
                    Vector2 position2D = new Vector2(transform.position.x, transform.position.y);

                    Vector2 directionToPlayer = new Vector2(transform.position.x, transform.position.y) - bottomLeft;
                    float angle = Vector2.Angle(directionToPlayer, bottomRight - bottomLeft);
                    Debug.Log("Angle to wall: " + angle);
                }
			}*/
			prevVelocityX = m_Rigidbody2D.velocity.x;
		}

		if (limitVelOnWallJump)
		{
			jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.right.x;

			if (m_Rigidbody2D.velocity.y < -0.5f)
				limitVelOnWallJump = false;

			if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
			{
				canMove = true;
			}
			else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
			{
				canMove = true;
				m_Rigidbody2D.velocity = new Vector2(10f * transform.right.x, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX < -2f) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX > 0) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
		}
	}


    public void Move(float move, bool jump, bool dash)
	{
		if (canMove) {
			if (dash && canDash && !isWallSliding && 
				(playerBattle.battleState == PlayerBattle.BattleState.Idle || playerBattle.battleState == PlayerBattle.BattleState.OnWall))
			{
				Dash();
			}
			// If crouching, check to see if the character can stand up
			if (isDashing)
			{
                m_Rigidbody2D.velocity = new Vector2(transform.right.x * m_DashForce, 0);
			}
			//only control the player if grounded or airControl is turned on
			else if (m_Grounded || m_AirControl)
			{
				if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight && !isWallSliding)
				{
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight && !isWallSliding)
				{
					Flip();
				}
			}
			// If the player should jump...
			if (m_Grounded && jump)
			{
                // Add a vertical force to the player.
                turnAble = true;
                animator.SetTrigger("Jump");
                animator.SetBool("Grounded", m_Grounded);
                m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				canDoubleJump = true;
				particleJumpDown.Play();
				particleJumpUp.Play();
			}
			else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
			{
				canDoubleJump = false;
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
                animator.SetTrigger("Jump");
            }
			else if (m_WallInfront && !m_Grounded)
			{
				if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
				{
					// Debug.Log("OnWall");
					isWallSliding = true;
					m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
					Flip();
					StartCoroutine(WaitToCheck(0.1f));
					canDoubleJump = true;
					animator.SetBool("WallSlide", true);
				}
				isDashing = false;

				if (isWallSliding)
				{
                    playerBattle.battleState = PlayerBattle.BattleState.OnWall;

					if (move * transform.right.x > 0.1f)
					{
						Invoke(nameof(WaitToEndSliding), endSlidingDelay);
					}
					else 
					{
						oldWallSlidding = true;
						m_Rigidbody2D.velocity = new Vector2(-transform.right.x * 2, 0);
						if(!onWallGravity)
							m_Rigidbody2D.simulated = false;
                    }
				}

				if (jump && isWallSliding)
				{
                    m_Rigidbody2D.simulated = true;
					animator.SetTrigger("Jump");
                    playerBattle.battleState = PlayerBattle.BattleState.Idle;

                    m_Rigidbody2D.velocity = new Vector2(0f, 0f);
					m_Rigidbody2D.AddForce(new Vector2(transform.right.x * m_JumpForce * wallJumpForceX, m_JumpForce));

					jumpWallStartX = transform.position.x;
					limitVelOnWallJump = true;
					canDoubleJump = true;
					isWallSliding = false;
					animator.SetBool("WallSlide", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canMove = false;
				}
				else if (dash && canDash)
				{
                    playerBattle.battleState = PlayerBattle.BattleState.Idle;
                    m_Rigidbody2D.simulated = true;

                    isWallSliding = false;
					animator.SetBool("WallSlide", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canDoubleJump = true;
					Dash();
				}
			}
			else if (isWallSliding && !m_WallInfront && canCheck) 
			{
				isWallSliding = false;
                m_Rigidbody2D.simulated = true;
                playerBattle.battleState = PlayerBattle.BattleState.Idle;

                animator.SetBool("WallSlide", false);
				oldWallSlidding = false;
				m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
				canDoubleJump = true;
			}
		}
	}


	private void Flip()
	{
		if (turnAble)
		{
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;
			transform.right *= -1;
			// Multiply the player's x local scale by -1.
			/*Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;*/
		}
	}


    #region Dash
    private void Dash()
	{
        animator.SetTrigger("Roll");
        isDashing = true;
        canDash = false;

        Invoke(nameof(DashDuration), dashDuration);
    }
	private void DashDuration()
	{
        isDashing = false;
        Invoke(nameof(ResetCanDash), dashCooldown);
    }
	private void ResetCanDash()
	{
		if (m_Grounded || isWallSliding)
			canDash = true;
		else
			Invoke(nameof(ResetCanDash), Time.deltaTime);
    }
    #endregion
    #region StunPlayer
    public void Stun(float time)
	{
        canMove = false;
		Invoke(nameof(ResetCanMove), time);
    }
	private void ResetCanMove()
	{
        canMove = true;
    }
    #endregion


	IEnumerator WaitToCheck(float time)
	{
		canCheck = false;
		yield return new WaitForSeconds(time);
		canCheck = true;
	}

	private void WaitToEndSliding()
	{
        m_Rigidbody2D.simulated = true;
        canDoubleJump = true;
		isWallSliding = false;
		animator.SetBool("WallSlide", false);
		oldWallSlidding = false;
		m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
        playerBattle.battleState = PlayerBattle.BattleState.Idle;
    }

	public void Dead()
	{
        canMove = false;
    }

  void AE_SlideDust()
    {
		/*
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }*/
    }
}
