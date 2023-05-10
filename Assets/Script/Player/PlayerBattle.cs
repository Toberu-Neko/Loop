using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private float life = 10f; //Life of the player
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
    [SerializeField][Range(0f, 1f)] private float blockMoveSpeedMultiplier;
    [SerializeField][Range(0f, 1f)] private float normalBlockDmgMultiplier;
    [SerializeField] private float blockCooldown;
    private bool canBlock = true;
    private bool isBlocking = false;

    private bool normalBlock = false;
    private bool perfectBlock = false;

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
    private CinemachineImpulseSource impulseSoruce;

    private float blockTimer = 0f;

    public enum BattleState { Idle, GroundAttack, SkyAttack, PerfectBlock, Block, Dash, OnWall };

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
					PlayerStatus.instance.moveable = true;
                    PlayerStatus.instance.jumpAndDashAble = true;
                    PlayerStatus.instance.movementMultiplier = 1f;

                    break;

				case BattleState.GroundAttack:
                        PlayerStatus.instance.moveable = false;
                    break;

                case BattleState.SkyAttack:
                    break;

                case BattleState.Block:
                    PlayerStatus.instance.jumpAndDashAble = false;
                    PlayerStatus.instance.movementMultiplier = blockMoveSpeedMultiplier;
					break;
			}
		}
		previousState = battleState;

        //Normal Attack
        if (normalAttack.WasPressedThisFrame() && canAttack && battleState == BattleState.Idle)
		{
            if (characterController.GetGrounded())
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

                battleState = BattleState.GroundAttack;

			    Invoke(nameof(AttackCooldown), attackCooldown);
            }
            if (!characterController.GetGrounded())
            {
                // Debug.Log("SkyAttack");
                animator.SetBool("SkyAttack", true);
                animator.SetTrigger("SkyAttackTrigger");
                characterController.turnAble = false;
                canAttack = false;

                DoAttack();

                battleState = BattleState.SkyAttack;

                ResetSkyAttack();
            }
        }

        //Sub Attack
		if (subAttack.WasPressedThisFrame() && battleState == BattleState.Idle)
		{
			GameObject throwableWeapon = Instantiate(throwableObject, attackCheck.position, Quaternion.identity); 
			Vector2 direction = new Vector2(transform.right.x , 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			throwableWeapon.name = "ThrowableWeapon";
		}

        //Block
        if(block.WasPressedThisFrame() && battleState == BattleState.Idle && canBlock)
        {
            animator.SetBool("IdleBlock", true);
            battleState = BattleState.Block;
            normalBlock = true;
            perfectBlock = true;
            canBlock = false;
            isBlocking = true;
        }

		if (block.IsPressed() && isBlocking)
		{
            blockTimer += Time.deltaTime;
            
            if (blockTimer > perfectBlockTime && perfectBlock)
                perfectBlock = false;

		}
		
        if (block.WasReleasedThisFrame() && isBlocking) 
		{
            //Perfect Block
            if (blockTimer <= perfectBlockTime)
			{
                animator.SetTrigger("Block");
                animator.SetBool("IdleBlock", false);
                normalBlock = false;
                perfectBlock = false;
                isBlocking = false;
                PlayerStatus.instance.moveable = false;
                Invoke(nameof(EndBlock), 0.417f);
            }
            else
            {
                //End of Block
	            animator.SetBool("IdleBlock", false);
                isBlocking = false;
                normalBlock = false;
                EndBlock();
            }

            blockTimer = 0;
        }
	}
    private void EndBlock()
    {
        battleState = BattleState.Idle;
        Invoke(nameof(ResetCanBlock), blockCooldown);
    }
    private void ResetCanBlock()
    {
        canBlock = true;
    }
    private void ResetSkyAttack()
    {
        if(characterController.GetGrounded() || characterController.GetWallSliding())
        {
            animator.SetBool("SkyAttack", false);
            characterController.turnAble = true;
            battleState = BattleState.Idle;
            canAttack = true;
        }
        else
        {
            Invoke(nameof(ResetSkyAttack), Time.deltaTime); 
            return;
        }
    }
    private void AttackFinished()
    {
        characterController.turnAble = true;
    }

    private void AttackCooldown()
	{
		if(battleState == BattleState.GroundAttack)
			battleState = BattleState.Idle;
        canAttack = true;
	}
    public void ApplyDamage(float damage, Vector3 position)
    {
        if (invincible)
            return;

        float applyDamage = damage;
        if (perfectBlock)
        {
            impulseSoruce.GenerateImpulse();
            return;
        }
        
        if(normalBlock)
        {
            applyDamage *= normalBlockDmgMultiplier;
        }

        animator.SetTrigger("Hurt");

        life -= applyDamage;

        Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;

        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(damageDir.x, 0) * 10);

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
    private void DoAttack()
    {
		bool haveEnemy = false;

        Vector2 attackSize = new(attackWidth, attackHeight);
		Vector2 attackOffset = new(attackWidth / 2, attackHeight / 2);
        Vector2 attackPosition = new(transform.position.x + attackOffset.x * transform.right.x,
                                             transform.position.y + attackOffset.y);

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
