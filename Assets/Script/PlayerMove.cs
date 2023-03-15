using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMove;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float movenSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpCD;
    [SerializeField]
    private float dashCD;

    private PlayerInput playerInput;
    private InputAction Move;
    private InputAction Jump;
    private Rigidbody2D playerRig;
    private GameObject groundDetector;
    private InputAction Dash;
    private float drag;
    private bool isDoubleJumpAble;
    private bool isGrounded;
    private bool isJumpAble;
    private bool isDashAble;
    public Facing facing;
    public enum Facing
    {
        Left,
        Right
    }

    private void Awake()
    {
        groundDetector = transform.Find("Misc/GroundDetector").gameObject;
        playerRig = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        Move = playerInput.actions["Move"];
        Jump = playerInput.actions["Jump"];
        Dash = playerInput.actions["Dash"];

        drag = playerRig.drag;
    }
    // Start is called before the first frame update
    void Start()
    {
        facing = Facing.Right;
        isJumpAble = true;
        isDoubleJumpAble = true;
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(new Vector2(groundDetector.transform.position.x, groundDetector.transform.position.y), Vector2.down, .05f, LayerMask.GetMask("Ground"), Mathf.Infinity, Mathf.Infinity);
        if (!isGrounded && playerRig.drag != 0)
            playerRig.drag = 0;
        else if(isGrounded && playerRig.drag == 0)
            playerRig.drag = drag;
        if (Move.ReadValue<Vector2>().x > 0)
        {
            // Trun Right
            facing = Facing.Right;
            //transform.Translate(movenSpeed * Time.deltaTime, 0, 0);
            playerRig.velocity = new Vector2(movenSpeed, playerRig.velocity.y);

        }
        else if (Move.ReadValue<Vector2>().x < 0)
        {
            // Trun Left
            facing = Facing.Left;
            //transform.Translate(-movenSpeed * Time.deltaTime, 0, 0);
            playerRig.velocity = new Vector2(-movenSpeed, playerRig.velocity.y);
        }

        #region Jump
        if (Jump.WasPressedThisFrame() && isGrounded && isJumpAble)
        {
            isJumpAble = false;
            playerRig.velocity = new Vector2(0, jumpForce);
            CancelInvoke(nameof(ResetJump));
            Invoke(nameof(ResetJump), jumpCD);
        }
        else if (Jump.WasPressedThisFrame() && !isGrounded && isDoubleJumpAble)
        {
            isDoubleJumpAble = false;
            playerRig.velocity = new Vector2(0, jumpForce);
            CancelInvoke(nameof(ResetJump));
            Invoke(nameof(ResetJump), jumpCD);
        }
        #endregion

        if (Dash.WasPressedThisFrame() && isDashAble)
        {
            isDashAble = false;
            
            // TODO: Dash and invulnerable. 
            
            Invoke(nameof(ResetDash), dashCD);
        }

    }
    private void ResetDash()
    {
        isDashAble = true;
    }
    private void ResetJump()
    {
        if (!isGrounded)
        {
            CancelInvoke(nameof(ResetJump));
            Invoke(nameof(ResetJump), Time.deltaTime);
            return;
        }
        isJumpAble = true;
        isDoubleJumpAble = true;
    }

}
