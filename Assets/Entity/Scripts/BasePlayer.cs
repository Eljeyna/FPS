using UnityEngine;

public class BasePlayer : BaseEntity
{
    public HealthGUI healthText;

    private new void Awake()
    {
        base.Awake();
    }

    public new void TakeDamage(float amount)
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

    public new void Die()
    {
        health = maxHealth;
        healthText.ChangeText("100");
    }
}
