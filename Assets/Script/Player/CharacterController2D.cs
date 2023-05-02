using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool m_AirControl = true;							// Whether or not a player can steer while jumping;


	
	[Header("Jump")]
    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_GroundLayer;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_WallCheck;                             //Posicion que controla si el personaje toca una pared
	
	[Header("WallJump")]
	[SerializeField] private float wallJumpForceX;
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

    private void Awake()
	{
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
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_GroundLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)				//If player is grounded
				m_Grounded = true;
				if (!wasGrounded )									//If player just landed
				{
					// OnLandEvent.Invoke();
					animator.SetBool("Grounded", m_Grounded);

					if (!m_WallInfront && !isDashing)				//If player is not beside a wall or dashing
						canDoubleJump = true;

					if (m_Rigidbody2D.velocity.y < 0f)
						limitVelOnWallJump = false;
				}
		}

        m_WallInfront = false;

		if (!m_Grounded)
		{
            animator.SetBool("Grounded", m_Grounded);

            Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_GroundLayer);
			for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != null)
				{
					isDashing = false;
					m_WallInfront = true;
				}
			}
			prevVelocityX = m_Rigidbody2D.velocity.x;
		}

		if (limitVelOnWallJump)
		{
			jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;

			if (m_Rigidbody2D.velocity.y < -0.5f)
				limitVelOnWallJump = false;

			if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
			{
				canMove = true;
			}
			else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
			{
				canMove = true;
				m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
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
			if (dash && canDash && !isWallSliding)
			{
				Dash();
			}
			// If crouching, check to see if the character can stand up
			if (isDashing)
			{
                // playerBattle.DoDashDamage();
                m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
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

					if (move * transform.localScale.x > 0.1f)
					{
						StartCoroutine(WaitToEndSliding());
					}
					else 
					{
						oldWallSlidding = true;
						m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, 0);
						m_Rigidbody2D.simulated = false;
                    }
				}

				if (jump && isWallSliding)
				{
                    m_Rigidbody2D.simulated = true;
					animator.SetTrigger("Jump");
                    playerBattle.battleState = PlayerBattle.BattleState.Idle;

                    m_Rigidbody2D.velocity = new Vector2(0f, 0f);
					m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_JumpForce * wallJumpForceX, m_JumpForce));

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
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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

	IEnumerator WaitToEndSliding()
	{
		//The delay of leaving wall.
		yield return new WaitForSeconds(0.1f);
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
    {/*
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
