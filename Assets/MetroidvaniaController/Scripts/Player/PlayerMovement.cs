using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

    private PlayerInput playerInput;
    private InputAction move;
    private InputAction jump;
    private InputAction dash;
    public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool goJump = false;
	bool goDash = false;

    //bool dashAxis = false;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
        jump = playerInput.actions["Jump"];
        dash = playerInput.actions["Dash"];
    }
    void Update () {

		//horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		horizontalMove = move.ReadValue<Vector2>().x * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (jump.WasPressedThisFrame())
		{
			goJump = true;
		}

		if (dash.WasPressedThisFrame())
		{
			goDash = true;
		}
	}

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, goJump, goDash);
		goJump = false;
		goDash = false;
	}
}
