using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The fake fly effect of the boss.
/// </summary>
public class FakeFly : MonoBehaviour
{
    [SerializeField] private AnimationCurve speed;
    [SerializeField] private float speedMultiplier = 0.5f;
    [SerializeField] private float frequency = 0.75f;

    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        float _speed = speed.Evaluate(timer);
        transform.position += _speed * speedMultiplier * Time.deltaTime * Vector3.up;

        timer += Time.deltaTime * frequency;
    }
}
