using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private MovementStyle movementStyle;
    private enum MovementStyle
    {
        Constant,
        Trigger
    }
    [SerializeField] private bool canMove;

    [SerializeField] private float speed;
    [SerializeField] private int startPoint;
    [SerializeField] private Transform[] points;


    [SerializeField] private Transform originalParent;
    private int count;
    private bool reverse;
    private bool isDisabling;

    private void Awake()
    {
        transform.position = points[startPoint].position;
        count = startPoint;
        reverse = false;
        isDisabling = false;
    }

    private void Update()
    {
        if(movementStyle == MovementStyle.Constant)
        {
            ConstantMovement();
        }
        else if(movementStyle == MovementStyle.Trigger)
        {
            TriggerMovement();
        }

    }

    private void ConstantMovement()
    {
        if (Vector2.Distance(transform.position, points[count].position) < 0.01f)
        {
            CheckNextPoint();
        }

        transform.position = Vector2.MoveTowards(transform.position, points[count].position, speed * Time.deltaTime);
    }

    private void TriggerMovement()
    {
        if (Vector2.Distance(transform.position, points[count].position) < 0.01f && canMove)
        {
            canMove = false;
            CheckNextPoint();
        }

        if (canMove) 
            transform.position = Vector2.MoveTowards(transform.position, points[count].position, speed * Time.deltaTime);
    }

    private void CheckNextPoint()
    {

        if (count == points.Length - 1)
        {
            reverse = true;
            count--;
            return;
        }
        else if (count == 0)
        {
            reverse = false;
            count++;
            return;
        }

        if (reverse)
        {
            count--;
        }
        else
        {
            count++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.transform.SetParent(BaseTempParent.Instance.transform);
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDisabling)
        {
            collision.transform.SetParent(null);
            gameObject.transform.SetParent(originalParent);
        }
    }

    private void OnDisable()
    {
        isDisabling = true;
    }
}
