public class BasePlayer : BaseEntity
{
    public HealthGUI healthText;

    public override void Awake()
    {
        health = maxHealth;
    }

    public override void TakeDamage(float amount, BaseEntity attacker)
    {
        health -= amount;
        this.attacker = attacker;

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
