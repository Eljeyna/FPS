﻿using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius;
    public float maxDistance;
    public float damagePrimary;
    public float damageSecondary;
    public float attackRangePrimary;
    public float attackRangeSecondary;
    public float fireRatePrimary;
    public float fireRateSecondary;
    public float impactForce;
    public LayerMask ENTITY_MASK;
    public Transform target;
    public Transform eyesPosition;
    public float nextAttack;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null)
        {
            Collider[] findTarget = Physics.OverlapSphere(transform.position, lookRadius, ENTITY_MASK);

            if (findTarget.Length == 0)
                return;

            foreach(Collider enemyTarget in findTarget)
            {
                if (findTarget[0] != enemyTarget)
                {
                    target = enemyTarget.transform;
                    break;
                }
            }

            if (target == null)
                return;
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance > maxDistance)
        {
            target = null;
            return;
        }

        agent.SetDestination(target.position);

        if (distance <= attackRangePrimary)
        {
            FaceToFace();
            PrimaryAttack();
        }
    }

    public void PrimaryAttack()
    {
        if (nextAttack > Time.time)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(eyesPosition.position, transform.forward, out hit, attackRangePrimary))
        {
            BaseEntity entity = hit.transform.GetComponent<BaseEntity>();
            if (entity != null)
            {
                if (entity.IsPlayer())
                {
                    BasePlayer player = (BasePlayer)entity;
                    player.TakeDamage(damagePrimary);
                }
                else
                    entity.TakeDamage(damagePrimary);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
