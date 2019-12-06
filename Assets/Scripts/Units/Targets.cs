using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Targets : MonoBehaviour
{
    public Unit_Base ub;
    public NavMeshAgent agent;
    /*
    public Transform[] points;
    int randomPoint;

    Transform nextPoint;
    public float startWaitingTime;                              //lo que ponemos en el editor, maximo tiempo de espera
    float waitingTime;
    */
    public Transform objetivo;
    public Transform[] targets;
    public float offSet = 1;
    private int initialPos = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        objetivo = targets[0];
        // randomPoint = Random.Range(0, points.Length);
        // waitingTime = startWaitingTime;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(objetivo.position);
        Vector3 distancia;
        distancia = objetivo.position - transform.position; //hayamos la distancia del punto del que quiero llegar al punto de donde estoy
        if (distancia.magnitude <= offSet)          //con la magnitud sabes la distancia que hay entre ambos
        {
            initialPos++;
            if (initialPos >= targets.Length)       //evitar pasarse de largo
            {
                initialPos = 0;
            }
            objetivo = targets[initialPos];         //objetivo igual al taget
        }
    }
}
