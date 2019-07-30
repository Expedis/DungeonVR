﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoll : MonoBehaviour,IDamage
{
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP;


    void Start()
    {
        currentHP = maxHP;
    }


    public void TakeDamage(float amount, Vector3 position)
    {
        currentHP -= amount;
        Debug.Log(amount);
        Debug.Log(currentHP);
        ShowDamageText(amount);
    }

    private void ShowDamageText(float amount)
    {
        GameObject damageText = ObjectPooler.instance.GetPooledObject("DamageText");
        damageText.transform.position = new Vector3(transform.position.x, transform.position.y + 4f,transform.position.z);
     //   damageText.transform.rotation = Quaternion.identity;
        damageText.transform.rotation = Camera.main.transform.rotation;
        damageText.SetActive(true);
        damageText.GetComponent<TextMesh>().text = "-" + amount.ToString();
    }
}