using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Referanse til agenten vår
    private NavMeshAgent agent;

    // Referanse til patrol points
    [SerializeField] private Transform[] patrolPoints;

    // Vite hvilket patrol point vi er på vei til nå
    private int currentPatrolPoint = 0;

    // Referanse til målet agenten skal følge
    [SerializeField] private Transform followTarget;

    

    private enum States
    {
        Patrolling,
        Chasing,
    }

    private States states;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        states = States.Patrolling;
    }


    // Kontinuerlig gi agenten vår et mål den skal prøve å nå
    private void Update()
    {
        Debug.Log(states);
         switch (states)
        {
            case States.Patrolling:
                Patrolling();
                break;
            case States.Chasing:
                Chasing();
                break;
        }
    }

    // Metode for Patrolling
    private void Patrolling()
    {
        // Vite hvor mange patrol points som finnes i arrayen vår.
        int numberOfPatrolPoints = patrolPoints.Length;

        // Hvis vi nærmer oss neste patrol point, så vil setter vi nytt mål
        if(agent.remainingDistance < 0.5f)
        {
            // Hvis nåværende mål er det siste i arrayen, så vil vi gjøre noe
            if(currentPatrolPoint + 1 == numberOfPatrolPoints)
            {
                // Sette currentPatrolPoints tilbare til 0
                currentPatrolPoint = 0;
                // Sett currentPatrolPoint (dom nå er 0) til vårt nye mål
                agent.SetDestination(patrolPoints[0].position);
            } else
            {
                // Øker vi currentPatrolPoint med 1
                currentPatrolPoint++;
                // Sett currentPatrolPoint som det nye målet vårt.
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            }
        }
    }

    // Metode for Chasing
    private void Chasing()
    {
        agent.SetDestination(followTarget.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            states = States.Chasing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            states = States.Patrolling;
        }
    }
}
