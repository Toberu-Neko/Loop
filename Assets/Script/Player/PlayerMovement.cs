using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D controller;
    [SerializeField] private float runSpeed = 40f;
	
	

    private PlayerInput playerInput;
    private InputAction move;
    private InputAction jump;
    private InputAction dash;

	private Animator animator;
	private Rigidbody2D rig;

	float horizontalMove = 0f;
	bool goJump = false;
	bool goDash = false;
	bool preFrameMoved = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
        jump = playerInput.actions["Jump"];
        dash = playerInput.actions["Dash"];
    }
    void Update () {

		horizontalMove = move.ReadValue<Vector2>().x * runSpeed;
        if (jump.WasPressedThisFrame())
        {
            goJump = true;
        }
        else if (dash.WasPressedThisFrame())
        {
            goDash = true;
        }
        else if (horizontalMove != 0 && !preFrameMoved)
		{
			preFrameMoved = true;
        }
        else if (horizontalMove == 0)
        {
			preFrameMoved = false;
            animator.SetInteger("AnimState", 0);
        }
		animator.SetFloat("AirSpeedY", rig.velocity.y);
	}

	public void OnFall()
	{
		animator.SetTrigger("Jump");
	}

	void FixedUpdate ()
	{
		if (PlayerStatus.moveable == false)
        {
            controller.Move(0, false, false);
            animator.SetInteger("AnimState", 0);
            goJump = false;
            goDash = false;
        }
        else
        {
		    // Move our character
		    controller.Move(horizontalMove * Time.fixedDeltaTime, goJump, goDash);
            if(horizontalMove != 0)
                animator.SetInteger("AnimState", 1);
            else
                animator.SetInteger("AnimState", 0);

            goJump = false;
		    goDash = false;
        }
	}
}
