using UnityEngine;

public class BasePlayer : BaseEntity
{
    public HealthGUI healthText;

    public override void Awake()
    {
        health = maxHealth;
    }

    public override void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Die();
            return;
        }

        healthText.ChangeText(health);
    }

    public override void Die()
    {
        health = maxHealth;
        healthText.ChangeText("100");
    }
}
