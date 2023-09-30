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

        anim.SetTrigger("damage");
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void GotoKinematicState(float time)
    {
        Debug.LogError("CombatTestDummy don't have kinematic state!");
    }

    public void GoToStunState()
    {
        Debug.LogError("CombatTestDummy don't have kinematic state!");
    }
}
