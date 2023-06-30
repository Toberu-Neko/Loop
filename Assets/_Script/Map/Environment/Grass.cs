using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem leafParticle;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.position.x - col.transform.position.x > 0) 
        {
            animator.Play("MovingGrassL");
        }
        else 
        {
            animator.Play("MovingGrassR");
        }
    }

    public void Damage(float damageAmount, Vector2 damagePosition, bool blockable = true)
    {
        Instantiate(leafParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}