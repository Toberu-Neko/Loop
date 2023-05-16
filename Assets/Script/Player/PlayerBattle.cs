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
    private bool canCounterAttack = false;
    private bool normalBlock = false;
    private bool perfectBlock = false;
    [SerializeField] private float counterAttackCountdown;

    [HideInInspector] public BattleState battleState = BattleState.Idle;
	private BattleState previousState;


    private bool canAttack = true;
    private CharacterController2D characterController;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private GameObject throwableObject;

    private CinemachineImpulseSource impulseSoruce;
    private Animator animator;
    private Collider2D playerCol;
    private Rigidbody2D rig;
    private PlayerUI playerUI;
    private PlayerStatus playerStatus;
    private Camera cam;

    private PlayerInput playerInput;
    private InputAction normalAttack;
    private InputAction subAttack;
    private InputAction block;
    private int attackCount = 1;

    private float blockTimer = 0f;

    [SerializeField] private GameObject showCounterAttack;
    public enum BattleState { Idle, GroundAttack, SkyAttack, CounterAttack, PerfectBlock, Block, Dash, OnWall };

    private void OnDrawGizmosSelected()
    {
        // 繪製攻擊範圍
        Gizmos.color = Color.red;

        // 計算攻擊框位置和大小
        Vector2 attackSize = new Vector2(attackWidth, attackHeight);
        Vector2 attackOffset = new Vector2(attackWidth / 2, attackHeight / 2);
        Vector2 attackPosition = new Vector2(transform.position.x + attackOffset.x * transform.right.x,
                                                 transform.position.y + 0.7f);

        // 繪製攻擊框
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
    private void Awake()
	{
        cam = Camera.main;
        playerStatus = GetComponent<PlayerStatus>();
        playerUI = GetComponent<PlayerUI>();
        showCounterAttack = transform.Find("DebugCounterAttack").gameObject;
        showCounterAttack.SetActive(false);

        playerCol = GetComponent<Collider2D>(); 
        characterController = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        impulseSoruce = GetComponent<CinemachineImpulseSource>();
        normalAttack = playerInput.actions["NormalAttack"];
        subAttack = playerInput.actions["SubAttack"];
		block = playerInput.actions["Block"];
    }
    private void Start()
    {
		battleState = BattleState.Idle;
        playerUI.UpdateLifeText(life);
    }
    void Update()
    {
		if(previousState != battleState) 
		{
            switch (battleState)
			{
                case BattleState.OnWall:
                case BattleState.Idle:
                    playerStatus.moveable = true;
                    playerStatus.jumpAndDashAble = true;
                    playerStatus.movementMultiplier = 1f;
                    rig.simulated = true;
                    invincible = false;
                    break;

				case BattleState.GroundAttack:
                    playerStatus.moveable = false;
                    break;

                case BattleState.SkyAttack:
                    // PlayerStatus.instance.moveable = false;
                    break;

                case BattleState.CounterAttack:
                    playerStatus.moveable = false;
                    rig.velocity = Vector2.zero;
                    rig.simulated = false;
                    invincible = true;
                    break;


                case BattleState.Block:
                    playerStatus.jumpAndDashAble = false;
                    playerStatus.movementMultiplier = blockMoveSpeedMultiplier;
					break;
			}
		}
		previousState = battleState;

        //Normal Attack
        if (normalAttack.WasPressedThisFrame() && canAttack && battleState == BattleState.Idle)
		{
            if (canCounterAttack)
            {
                animator.SetTrigger("CounterAttackTrigger");
                canAttack = false;
                DoAttack(100f, 100f);
                battleState = BattleState.CounterAttack;
                Invoke(nameof(RestCanCounterAttack), 0.571f);
            }
            else if (!canCounterAttack && characterController.GetGrounded())
            {
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
            else if (!canCounterAttack && !characterController.GetGrounded())
            {
                // Debug.Log("SkyAttack");
                animator.SetBool("SkyAttack", true);
                animator.SetTrigger("SkyAttackTrigger");
                
                canAttack = false;

                DoAttack();

                battleState = BattleState.SkyAttack;

                ResetSkyAttack();
            }
        }

        //Sub Attack
		if (subAttack.WasPressedThisFrame() && battleState == BattleState.Idle)
		{
            Vector3 worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
			GameObject throwableWeapon = Instantiate(throwableObject, attackCheck.position, Quaternion.identity); 
			Vector2 direction = worldMousePos - transform.position;

            throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction.normalized; 
			throwableWeapon.name = "ThrowableWeapon";
		}

        //Block
        #region Block
        if (block.WasPressedThisFrame() && battleState == BattleState.Idle && canBlock)
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
            if(battleState != BattleState.Block)
            {
                ReleaseBlock(false);
                return;
            }

            blockTimer += Time.deltaTime;
            
            if (blockTimer > perfectBlockTime && perfectBlock)
                perfectBlock = false;

		}
		
        if (block.WasReleasedThisFrame() && isBlocking) 
		{
            ReleaseBlock(false);
        }
        #endregion
    }
    public void ApplyDamage(float damage, Vector3 position, bool canBlock = true)
    {
        if (invincible)
            return;

        float applyDamage = damage;
        if (perfectBlock && canBlock)
        {
            impulseSoruce.GenerateImpulse();
            canCounterAttack = true;

            ReleaseBlock(true);
            showCounterAttack.SetActive(true);

            Invoke(nameof(RestCanCounterAttack), counterAttackCountdown);

            IgnoreAttack(damagedInvincibleTime);
            return;
        }

        if (normalBlock && canBlock)
        {
            applyDamage *= normalBlockDmgMultiplier;
        }

        animator.SetTrigger("Hurt");

        life -= applyDamage;
        playerUI.UpdateLifeText(life);

        Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;

        rig.velocity = Vector2.zero;
        rig.AddForce(new Vector2(damageDir.x, 0) * 10);

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
    #region BlockF
    private void ReleaseBlock(bool doPerfect)
    {
        //Perfect Block
        if (blockTimer <= perfectBlockTime && doPerfect)
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", false);
            normalBlock = false;
            perfectBlock = false;
            isBlocking = false;
            playerStatus.moveable = false;
            Invoke(nameof(EndBlock), 0.417f);//0.417f
        }
        else
        {
            //End of Block
            animator.SetBool("IdleBlock", false);
            isBlocking = false;
            normalBlock = false;
            perfectBlock = false;
            EndBlock();
        }

        blockTimer = 0;
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
    #endregion

    private void DoAttack(float t_setWidth = 0f, float t_setHeight = 0f)
    {
        characterController.turnAble = false;

        bool haveEnemy = false;
        Vector2 attackSize;
        if (t_setWidth == 0f || t_setHeight == 0f)
            attackSize = new(attackWidth, attackHeight);
        else
            attackSize = new(t_setWidth, t_setHeight);

        Vector2 attackOffset = new(attackSize.x / 2, attackSize.y / 2);
        Vector2 attackPosition = new(transform.position.x + attackOffset.x * transform.right.x,
                                     transform.position.y + 0.7f);

        Collider2D[] collidersEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0);
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
    void RestCanCounterAttack()
    {
        CancelInvoke(nameof(RestCanCounterAttack));
        if(battleState == BattleState.CounterAttack)
        {
            canAttack = true;
            battleState = BattleState.Idle;
            characterController.turnAble = true;
        }

        canCounterAttack = false;
        showCounterAttack.SetActive(false);
    }
    
    private void IgnoreAttack(float time)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8, true);
        Invoke(nameof(ResetIgnoreAttack), time);
    }
    private void ResetIgnoreAttack()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8, false);
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
        rig.velocity = new Vector2(0, rig.velocity.y);
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
