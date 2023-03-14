using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction Move;
    private InputAction Jump;
    private Rigidbody2D playerRig;
    private Transform groundDetector;
    [SerializeField]
    private float movenSpeed;
    [SerializeField]
    private float jumpForce;
    private bool grounded;


    private bool jumpAble;

    private void Awake()
    {
        groundDetector = transform.Find("Misc/GroundDetector");
        playerRig = transform.Find("Model").gameObject.GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        Move = playerInput.actions["Move"];
        Jump = playerInput.actions["Jump"];
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpAble = true;
    }
    public void OnWASD()
    {
    }
    // Update is called once per frame
    void Update()
    {
        //grounded = Physics2D.Raycast( Vector2.down, movenSpeed, LayerMask.GetMask("Ground"))
        if (Move.ReadValue<Vector2>().x > 0)
        {
            transform.Translate(movenSpeed * Time.deltaTime, 0, 0);
            
        }
        else if(Move.ReadValue<Vector2>().x < 0)
        {
            transform.Translate(-movenSpeed * Time.deltaTime, 0, 0);
        }

        if (Jump.IsPressed() && jumpAble)
        {
            jumpAble = false;
            playerRig.velocity = new Vector2(0, jumpForce);
        }
    }
}
