﻿using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;
    public bool flagDeath;
    public BaseEntity attacker;

    public abstract void Awake();
    public abstract void TakeDamage(float amount, BaseEntity attacker);
    public abstract void Die();
}
