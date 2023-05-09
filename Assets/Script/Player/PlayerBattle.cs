﻿using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private int life = 10; //Life of the player
    [HideInInspector] public bool invincible = false; //If player can be hurt
    [Header("Damaged")]
    [SerializeField] private float damagedStunTime = 0.25f;
    [SerializeField] private float damagedInvincibleTime = 1f;

    [Header("Attack")]
	[SerializeField] private float attackCooldown;
    [SerializeField] private float dmgValue;
	[SerializeField] private float attackWidth;
	[SerializeField] private float attackHeight;

    [Header("Block")]
	[SerializeField] private float perfectBlockTime;

    public BattleState battleState = BattleState.Idle;
	private BattleState previousState;

	private Rigidbody2D m_Rigidbody2D;
	private Animator animator;
    private bool canAttack = true;
    private CharacterController2D characterController;

    private PlayerInput playerInput;
    private InputAction normalAttack;
	private InputAction subAttack;
	private InputAction block;
	private int attackCount = 1;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private CameraFollow cameraFollow;
    private CinemachineImpulseSource impulseSoruce;

    private float blockTimer = 0f;

    public enum BattleState { Idle, Attack, Block, Dash, Jump, Fall, Hit, Dead, OnWall };

    private void OnDrawGizmosSelected()
    {
        // 繪製攻擊範圍
        Gizmos.color = Color.red;

        // 計算攻擊框位置和大小
        Vector2 attackSize = new Vector2(attackWidth, attackHeight);
        Vector2 attackOffset = new Vector2(attackWidth / 2, attackHeight / 2);
        Vector2 attackPosition = new Vector2(transform.position.x + attackOffset.x * transform.right.x,
                                                 transform.position.y + attackOffset.y);

        // 繪製攻擊框
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
    private void Awake()
	{
        characterController = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        impulseSoruce = GetComponent<CinemachineImpulseSource>();
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
                    break;
                case BattleState.Block:
					PlayerStatus.moveable = false;
					break;
			}
		}
		previousState = battleState;


        if (normalAttack.WasPressedThisFrame() && canAttack && battleState == BattleState.Idle)
		{
            characterController.turnAble = false;
            canAttack = false;
			animator.SetTrigger("Attack" + attackCount);
            if (attackCount == 1 || attackCount == 2)
                Invoke(nameof(AttackFinished), 0.429f);
            if(attackCount == 3)
                Invoke(nameof(AttackFinished), 0.571f);

            attackCount += 1;
			if(attackCount == 4)
                attackCount = 1;

			DoAttack();

            battleState = BattleState.Attack;

			Invoke(nameof(AttackCooldown), attackCooldown);
		}

		if (subAttack.WasPressedThisFrame() && battleState == BattleState.Idle)
		{
			GameObject throwableWeapon = Instantiate(throwableObject, attackCheck.position, Quaternion.identity); 
			Vector2 direction = new Vector2(transform.right.x , 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			throwableWeapon.name = "ThrowableWeapon";
		}

		if (block.IsPressed() && battleState != BattleState.OnWall && battleState != BattleState.Attack)
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
    private void AttackFinished()
    {
        characterController.turnAble = true;
    }

    private void AttackCooldown()
	{
		if(battleState == BattleState.Attack)
			battleState = BattleState.Idle;
        canAttack = true;
	}
    public void ApplyDamage(int damage, Vector3 position)
    {
        if (!invincible)
        {
            //animator.SetBool("Hit", true);
            animator.SetTrigger("Hurt");
            life -= damage;
            Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.AddForce(damageDir * 10);
            if (life <= 0)
            {
                life = 0;

                StartCoroutine(WaitToDead());
            }
            else
            {
                characterController.Stun(damagedStunTime);
                StartCoroutine(MakeInvincible(damagedInvincibleTime));
            }
        }
    }
    private void DoAttack()
    {
		bool haveEnemy = false;

        Vector2 attackSize = new(attackWidth, attackHeight);
		Vector2 attackOffset = new(attackWidth / 2, attackHeight / 2);
        Vector2 attackPosition = new(transform.position.x + attackOffset.x * transform.right.x,
                                             transform.position.y + attackOffset.y);

        // Check enemies in attack range
        Collider2D[] collidersEnemies = Physics2D.OverlapBoxAll(attackPosition , attackSize, 0);
        foreach (Collider2D enemyCollider in collidersEnemies)
        {
            if (enemyCollider.gameObject.CompareTag("Enemy"))
            {
				haveEnemy = true;
                enemyCollider.gameObject.SendMessage("ApplyDamage", dmgValue);
            }
        }
		if (haveEnemy)
		{
            impulseSoruce.GenerateImpulse();
        }
    }
    IEnumerator MakeInvincible(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }
    IEnumerator WaitToDead()
    {
        SendMessage("Dead");
        animator.SetTrigger("Death");
        
        invincible = true;
        enabled = false;
        yield return new WaitForSeconds(0.4f);
        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.CompareTag("Enemy"))
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
                impulseSoruce.GenerateImpulse();
            }
		}
	}
}
