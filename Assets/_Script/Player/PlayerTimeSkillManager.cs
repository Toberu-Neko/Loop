using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkillManager : MonoBehaviour
{
    [SerializeField] private PlayerTimeSkillData data;

    private List<PointInTime> pointsInTime;
    private PlayerInputHandler inputHandler;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private Core core;
    private Movement movement;
    private Stats stats;

    private float maxEnergy;
    private float currentEnergy;


    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();

        maxEnergy = data.maxEnergy;
        currentEnergy = maxEnergy;

        pointsInTime = new();
    }

    private void Update()
    {
        if(inputHandler.TimeSkillInput && !stats.IsRewindingPosition)
        {
            inputHandler.UseTimeSkillInput();
            StartRewinding();
        }
        if(stats.IsRewindingPosition && !inputHandler.TimeSkillHoldInput)
        {
            StopRewinding();
            pointsInTime.Clear();
        }
    }

    private void FixedUpdate()
    {
        if(stats.IsRewindingPosition)
        {
            RewindPosition();
        }
        else
        {
            Record();
        }
    }

    private void Record()
    {
        pointsInTime.Insert(0, new PointInTime((Vector2)transform.position, transform.rotation, movement.FacingDirection, sr.sprite));
    }

    private void RewindPosition()
    {
        if(pointsInTime.Count > 0)
        {
            PointInTime point = pointsInTime[0];
            movement.SetPosition(point.position, point.rotation, point.facingDirection);
            sr.sprite = point.sprite;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewinding();
            return;
        }
    }
    private void StartRewinding()
    {
        stats.SetRewindingPosition(true);
        rb.simulated = false;
        anim.enabled = false;

        stats.SetInvincibleTrue();
    }
    private void StopRewinding()
    {
        stats.SetRewindingPosition(false);
        rb.simulated = true;
        anim.enabled = true;

        stats.SetInvincibleFalse();
    }
}

[Serializable]
public class PointInTime
{
    public Vector2 position;
    public Quaternion rotation;
    public int facingDirection;
    public Sprite sprite;

    public PointInTime(Vector2 position, Quaternion rotation, int facingDirection, Sprite sprite)
    {
        this.position = position;
        this.rotation = rotation;
        this.facingDirection = facingDirection;
        this.sprite = sprite;
    }
}
