using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;

    public void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public bool IsPlayer()
    {
        return (BasePlayer)this;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
