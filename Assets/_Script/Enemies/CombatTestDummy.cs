using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject hitParticles;
    private Animator anim;

    public void Damage(AttackDetails details)
    {
        Debug.Log(details.damageAmount);

        // Instantiate(hitParticles, transform.position, Quaternion.Euler(0f,0f,Random.Range(0f, 360f)));
        anim.SetTrigger("damage");
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
