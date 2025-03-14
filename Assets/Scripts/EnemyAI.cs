using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Referanse til agenten v�r
    private NavMeshAgent agent;

    // Referanse til patrol points
    [SerializeField] private Transform[] patrolPoints;

    // Vite hvilket patrol point vi er p� vei til n�
    private int currentPatrolPoint = 0;

    // Referanse til m�let agenten skal f�lge
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


    // Kontinuerlig gi agenten v�r et m�l den skal pr�ve � n�
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
        // Vite hvor mange patrol points som finnes i arrayen v�r.
        int numberOfPatrolPoints = patrolPoints.Length;

        // Hvis vi n�rmer oss neste patrol point, s� vil setter vi nytt m�l
        if(agent.remainingDistance < 0.5f)
        {
            // Hvis n�v�rende m�l er det siste i arrayen, s� vil vi gj�re noe
            if(currentPatrolPoint + 1 == numberOfPatrolPoints)
            {
                // Sette currentPatrolPoints tilbare til 0
                currentPatrolPoint = 0;
                // Sett currentPatrolPoint (dom n� er 0) til v�rt nye m�l
                agent.SetDestination(patrolPoints[0].position);
            } else
            {
                // �ker vi currentPatrolPoint med 1
                currentPatrolPoint++;
                // Sett currentPatrolPoint som det nye m�let v�rt.
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
