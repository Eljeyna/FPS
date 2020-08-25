public class BaseEnemy : BaseEntity
{
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
        }
    }

    public override void Die()
    {
        //Destroy(gameObject);
        flagDeath = true;
    }
}
