using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Patrolling, Alerted, TargetVisible, Dead
}
public class EnemyAIStateMachine : MonoBehaviour
{
    [SerializeField]
    private EnemyState currentState = EnemyState.Patrolling;

    [Header("Patrolling")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int waypointIndex = 0;
    [SerializeField] private bool patrolLoop = true;
    [SerializeField] private float closeEnoughDistance;

    [Header("Alerted")]
    [SerializeField] private float lastAlertTime = 0.0f;
    [SerializeField] private float alertCooldown = 8.0f;
    [SerializeField] private Vector3 lastKnownTargetPosition;

    [Header("Target in Sight")]
    [SerializeField] private float lastRunTime = 0.0f;
    [SerializeField] private float runCooldown = 1.0f;
    [SerializeField] private Transform target = null;
    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Start()
    {
        currentState = EnemyState.Patrolling;
        animator = GetComponent<Animator>();
        animator.SetFloat("Forward", 0.5f);
        agent = GetComponent<NavMeshAgent>();

        if (agent != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }

    public void Pause()
    {
        agent.enabled = false;
        animator.SetFloat("Forward", 0f);
    }

    public void Resume()
    {
        agent.enabled = true;
        animator.SetFloat("Forward", 1f);
    }

    public EnemyState GetState()
    {
        return currentState;
    }

    public void SetState(EnemyState newState)
    {
        if (currentState == newState)
            return;
        
        currentState = newState;
        
        if (newState == EnemyState.Patrolling)
        {
            // resume patrol
            agent.enabled = true;
            agent.SetDestination(waypoints[waypointIndex].position);
        }

        else if (newState == EnemyState.Alerted)
        {
            // investigate the last known position

            agent.enabled = true;
            agent.SetDestination(lastKnownTargetPosition);
            // remember when we were alerted
            lastAlertTime = Time.time;
        }

        else if (newState == EnemyState.TargetVisible)
        {
            // run at the player
            agent.enabled = false;
            animator.SetFloat("Forward", 4f);
            lastKnownTargetPosition = target.transform.position;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetLastKnownTargetPosition(Vector3 lastKnownTargetPosition)
    {
        this.lastKnownTargetPosition = lastKnownTargetPosition;
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead)
        {
            return;
        }

        else if (currentState == EnemyState.Patrolling)
        {
            Patrol();
        }

        else if (currentState == EnemyState.Alerted)
        {
            // check for timeout
            if (Time.time > (lastAlertTime + alertCooldown))
            {
                SetState(EnemyState.Patrolling);
                Patrol();
            }

            else
            {
                Alert();
            }
        }

        else if (currentState == EnemyState.TargetVisible)
        {
            Run();
        }
    }
    private void Patrol()
    {
        Vector3 destination = waypoints[waypointIndex].position;
        float distanceToTarget = Vector3.Distance(agent.transform.position, destination);
        
        if (distanceToTarget < closeEnoughDistance)
        {
            waypointIndex++;

            if (waypointIndex >= waypoints.Length)
            {
                if (patrolLoop)
                {
                    waypointIndex = 0;

                }

                else
                {
                    animator.SetFloat("Forward", 0f);
                    return;
                }
            }

            agent.SetDestination(waypoints[waypointIndex].position);
        }

        animator.SetFloat("Forward", 1f);
    }
    
    private void Alert()
    {
        float distanceToTarget = Vector3.Distance(agent.transform.position, lastKnownTargetPosition);
        if (distanceToTarget < closeEnoughDistance)
        {
            // stay here
            animator.SetFloat("Forward", 0.0f);
        }
        else
        {
            animator.SetFloat("Forward", 3f);
        }
    }
    private void Run()
    {
        if (Time.time > (lastRunTime + runCooldown))
        {
            lastRunTime = Time.time;
        }
    }
}