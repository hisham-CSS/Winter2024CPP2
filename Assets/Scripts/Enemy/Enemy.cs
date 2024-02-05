using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //reference to player or target if enemy gets aggro'd
    public GameObject target;
    
    NavMeshAgent agent;

    public enum EnemyState
    {
        Chase, Patrol
    }

    public EnemyState currentState;

    public GameObject[] path;
    public int pathIndex;
    public float distThreshold;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (distThreshold <= 0)
        {
            distThreshold = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EnemyState.Patrol)
        {
            if (target)
                Debug.DrawLine(transform.position, target.transform.position, Color.red);

            if (agent.remainingDistance < distThreshold)
            {
                pathIndex++;
                pathIndex %= path.Length;

                target = path[pathIndex];
            }
        }
        if (currentState == EnemyState.Chase)
        {
            if (target.CompareTag("PatrolNode"))
            {
                target = GameObject.FindGameObjectWithTag("Player");
            }
        }

        if (target)
            agent.SetDestination(target.transform.position);
    }
}
