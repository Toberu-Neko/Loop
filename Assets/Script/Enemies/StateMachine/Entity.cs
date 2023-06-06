using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public int lastDamageDirection { get; private set; }
    public Core core { get; private set; }

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    private Vector2 velocityWorkspcae;

    protected bool isStunned;
    protected bool isDead;

    public virtual void Awake()
    {
        core = GetComponentInChildren<Core>();

        facingDirection = 1;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }
}
