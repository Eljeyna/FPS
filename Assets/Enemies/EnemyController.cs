using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float maxDistance;
    public float damagePrimary;
    public float damageSecondary;
    public float attackRangePrimary;
    public float attackRangeSecondary;
    public float fireRatePrimary;
    public float fireRateSecondary;
    public float waitingTime;
    public float impactForce;
    public LayerMask ENTITY_MASK;
    public Transform target;
    public Transform eyesPosition;
    public float nextAttack;
    public float nextWait;

    private NavMeshAgent agent;
    private Animator animations;
    private BaseEnemy thisEnemy;
    private BoxCollider triggerEnemy;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<Animator>();
        thisEnemy = GetComponent<BaseEnemy>();
        triggerEnemy = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (thisEnemy.flagDeath)
        {
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            triggerEnemy.enabled = false;
            animations.SetInteger("Animation", 4);
            this.enabled = false;
            return;
        }

        if (nextWait > Time.time)
            return;

        if (target == null)
        {
            if (thisEnemy.attacker != null)
            {
                target = thisEnemy.attacker.gameObject.transform;
            }
            else
            {
                nextWait = Time.time + waitingTime;
                animations.SetInteger("Animation", 0);
                return;
            }
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance > maxDistance)
        {
            target = null;
            triggerEnemy.enabled = true;
            return;
        }

        agent.SetDestination(target.position);
        FaceToFace();
        if (nextAttack - fireRatePrimary / 4 <= Time.time)
        {
            animations.SetInteger("Animation", 2);
        }

        if (distance <= attackRangePrimary)
        {
            PrimaryAttack();
        }
    }

    public void PrimaryAttack()
    {
        if (nextAttack > Time.time)
        {
            return;
        }

        animations.SetInteger("Animation", 1);

        RaycastHit hit;
        if (Physics.Raycast(eyesPosition.position, transform.forward, out hit, attackRangePrimary))
        {
            BaseEntity entity = hit.transform.GetComponent<BaseEntity>();
            if (entity != null)
            {
                entity.TakeDamage(damagePrimary, thisEnemy);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.Impulse);
            }
        }

        nextAttack = Time.time + fireRatePrimary;
    }

    public void SecondaryAttack()
    {
        return;
    }

    public void FaceToFace()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEnemy.enabled = false;
        target = other.gameObject.transform;
    }
}
