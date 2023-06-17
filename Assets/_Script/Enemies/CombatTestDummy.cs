using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject hitParticles;
    private Animator anim;

    public void Damage(float damageAmount, Vector2 damagePosition, bool blockable = true)
    {
        Debug.Log(damageAmount);

        Instantiate(hitParticles, transform.position, Quaternion.Euler(0f,0f,Random.Range(0f, 360f)));
        anim.SetTrigger("damage");
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
