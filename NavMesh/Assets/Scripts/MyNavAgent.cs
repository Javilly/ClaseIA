using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyNavAgent : MonoBehaviour
{

    public Transform transformWeaLoca;
    public NavMeshAgent meshAgentoLoco;
    public GameObject[] puntosLocos = new GameObject[4];
    int i = 1;


    void Start()
    {
        meshAgentoLoco.SetDestination(puntosLocos[0].transform.position);
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, meshAgentoLoco.destination) <= 1)
        {
            meshAgentoLoco.SetDestination(puntosLocos[i].transform.position);
            i++;
        }
        if(i == 4)
        {
            i = 0;
        }
    }
}
