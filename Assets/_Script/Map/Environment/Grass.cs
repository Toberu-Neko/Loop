using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour, IMapDamageableItem
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

    public void TakeDamage(float damage)
    {
        Instantiate(leafParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}