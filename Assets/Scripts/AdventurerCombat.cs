﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerCombat : MonoBehaviour
{
  
    public float attackSpeed;
    public Transform attackPoint;
    public float attackRange;
    public float lastAttackRange;
    //private ActivateMelee meleeWeapon;
    private AdventurerState state;
    private Animator anim;
    private float attackCooldown;
    private int comboMeter = 0;
    private bool nexAttack = true;
    private ParticleSystem weaponTrail;
    private float oldAttackRange;
    private float oldAttackDamge;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        state = GetComponent<AdventurerState>();
        weaponTrail = GetComponentInChildren<ParticleSystem>();
        // meleeWeapon = GetComponentInChildren<ActivateMelee>();
        oldAttackRange = attackRange;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

    public void LightAttack()
    {
        if (attackCooldown <= 0 && nexAttack == true)
        {
            StartCoroutine("Attack");
            attackCooldown = 1f / attackSpeed;
        }
    }

    IEnumerator Attack()
    {
        weaponTrail.Play();
        nexAttack = false;
        StopCoroutine("ComboBreaker");
        state.isAttacking = true;
        comboMeter++;
        anim.SetTrigger("IsAttack" + comboMeter);
        if(comboMeter == 4)
        {
            attackRange = lastAttackRange;
        }
        yield return new WaitForSeconds(0.1f);
        var attack = Physics.OverlapBox(attackPoint.position, new Vector3(attackRange, attackRange, attackRange), Quaternion.identity , LayerMask.GetMask("Enemy"));
        foreach(var enemy in attack)
        {
            enemy.GetComponent<IDamage>().TakeDamage(state.attackDamage, transform.position);
        }
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Kończy atak " + comboMeter);
        nexAttack = true;
        StartCoroutine("ComboBreaker");
        if (comboMeter == 4)
        {
            comboMeter = 0;
        }
    }

    private IEnumerator ComboBreaker()
    {
        anim.SetBool("IsStance", true);
        anim.SetBool("IsRun", false);
        anim.SetBool("IsIdle", false);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("IsStance", false);
        anim.SetBool("IsIdle", true);
        state.isAttacking = false;
        Debug.Log("Reset comba");
        comboMeter = 0;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRange, attackRange, attackRange));
    }

}
