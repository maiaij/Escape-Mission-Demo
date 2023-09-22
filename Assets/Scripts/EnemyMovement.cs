using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float closeEnoughDistance;
    [SerializeField] bool loop;
    private NavMeshAgent agent = null;
    private Animator animator = null;
    private int wayPointIndex = 0;
    private bool patrolling = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if ((agent != null) && (waypoints.Length > 0))
        {
            agent.SetDestination(waypoints[wayPointIndex].position);
        }
    }

    private void Update()
    {
        if (!patrolling)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(agent.transform.position, waypoints[wayPointIndex].position);
        
        if (distanceToTarget < closeEnoughDistance)
        {
            // make the next waypoint active
            wayPointIndex++;
            if (wayPointIndex >= waypoints.Length)
            {
                if (loop)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    patrolling = false;
                    animator.SetFloat("Forward", 0);

                    return;
                }
            }
            // navigate to the waypoint
            agent.SetDestination(waypoints[wayPointIndex].position);
        }
        animator.SetFloat("Forward", agent.velocity.magnitude);
    }
}