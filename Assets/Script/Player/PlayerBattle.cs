using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerBattle;
using static PlayerMovement;

public class PlayerBattle : MonoBehaviour
{
	[Header("Attack")]
	[SerializeField] private float attackCooldown;
	[Header("Block")]
	[SerializeField] private float perfectBlockTime;
    [SerializeField] private float dmgValue;

    public static BattleState battleState = BattleState.Idle;
	private BattleState previousState;

	private Rigidbody2D m_Rigidbody2D;
	private Animator animator;
    [SerializeField] private bool canAttack = true;

    private PlayerInput playerInput;
    private InputAction normalAttack;
	private InputAction subAttack;
	private InputAction block;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private GameObject cam;

	private float blockTimer = 0f;

    public enum BattleState { Idle, Attack, Block, Dash, Jump, Fall, Hit, Dead, OnWall };

    private void Awake()
	{
        animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
		normalAttack = playerInput.actions["NormalAttack"];
        subAttack = playerInput.actions["SubAttack"];
		block = playerInput.actions["Block"];
    }
    private void Start()
    {
		battleState = BattleState.Idle;
    }
    void Update()
    {
		if(previousState != battleState) 
		{
			switch (battleState)
			{
				case BattleState.Idle:
					PlayerStatus.moveable = true;
					break;

				case BattleState.Attack:
				case BattleState.Block:
					PlayerStatus.moveable = false;
					break;
			}
		}
		previousState = battleState;


        if (normalAttack.WasPressedThisFrame() && canAttack && battleState == BattleState.Idle)
		{
			canAttack = false;
			animator.SetTrigger("Attack" + "1");
			battleState = BattleState.Attack;

			Invoke(nameof(AttackCooldown), attackCooldown);
		}

		if (subAttack.WasPressedThisFrame())
		{
			GameObject throwableWeapon = Instantiate(throwableObject, attackCheck.position, Quaternion.identity) as GameObject; 
			Vector2 direction = new Vector2(transform.localScale.x , 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			throwableWeapon.name = "ThrowableWeapon";
		}

		if (block.IsPressed())
		{
			if(blockTimer == 0)
			{
                animator.SetBool("IdleBlock", true);
                battleState = BattleState.Block;
				// Debug.Log("block");
            }
            blockTimer += Time.deltaTime;
		}
		else if (blockTimer != 0f) 
		{
			if(blockTimer <= perfectBlockTime)
			{
                animator.SetTrigger("Block");
                animator.SetBool("IdleBlock", false);
				
            }
			else
	            animator.SetBool("IdleBlock", false);

            blockTimer = 0;
			battleState = BattleState.Idle;
        }
	}

	private void AttackCooldown()
	{
		if(battleState == BattleState.Attack)
			battleState = BattleState.Idle;
        canAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}
}
