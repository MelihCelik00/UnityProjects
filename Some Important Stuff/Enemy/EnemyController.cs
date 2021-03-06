﻿using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float attackDistance;
    public float Health { get => health; }

    public Transform target;
    public NavMeshAgent agent;
    private Patrol patrolEnemy;

    public bool NotInArea = false;

    public bool isActive = false;

    private Animator anims;

    private EnemyAnim enemyAnimations;

    public bool MenzilCheck = false;//temporary variable
    
    private void Start()
    {
        target = PlayerManager.instance.player.transform; // Initialization
        agent = GetComponent<NavMeshAgent>();
        enemyAnimations = FindObjectOfType<EnemyAnim>();

        anims = GetComponentInChildren<Animator>();
    }

    private void Update() // bool döndür patrol scriptine yolla
    {
        //float distance = Vector3.Distance(target.position, transform.position);

        double distance = CalculateHorizontalDistance();
        
        if (distance <= agent.stoppingDistance)
        {
            agent.Stop();
            Attack();
            FaceTarget();
        }else if (distance <= attackDistance)
        {
            FollowPlayer();
            agent.transform.LookAt(target.position);
        }

        ////////////////
        /*
        if (distance <= attackDistance)
        {
            FollowPlayer();
            MenzilCheck = true;
            if (distance <= agent.stoppingDistance)
            {
                agent.Stop();
                Attack();
                

            }
            FaceTarget();
        } */ 
        //////////////
        
        /*
        if (distance == agent.stoppingDistance)
        {
            agent.Stop();
            Attack();
        }
        else if ( (agent.stoppingDistance < distance) && (distance <= attackDistance))
        {
            isActive = true;
            FollowPlayer();
            
            NotInArea = false;
        }
        */
        
        
        /*
        if (distance == agent.stoppingDistance) 
        {
            // Attack the target
            // Face the target
            //FaceTarget(); // IT WORKS!
            if (distance <= 2f)
            {
                //Attack();
            }
        }
        */

        if (distance > attackDistance)
        {
            NotInArea = true;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void FollowPlayer() // Enemy does not follow Player. Fix it!
    {
        anims.SetBool("ifAttack", false);
        anims.SetFloat("Movement", 2);
        agent.speed = 5;
        //agent.SetDestination(target.position);
        //agent.destination = target.position;
        agent.transform.position = Vector3.MoveTowards(agent.transform.position, target.position, agent.speed * Time.deltaTime);
        //FaceTarget();
        transform.LookAt(target.transform.position);
    }

    public void Attack()
    {
        anims.SetBool("ifAttack", true);
        anims.SetFloat("Movement", 0);
        float damage = 1;
        target.GetComponent<Player>().TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private double CalculateHorizontalDistance()
    {
        float xDistance = target.position.x - transform.position.x;
        float zDistance = target.position.z - transform.position.z;
        
        
        double distance = Math.Sqrt( (xDistance * xDistance) + (zDistance * zDistance) );

        return distance;
    }

}