using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementManager : MonoBehaviour
{
    [SerializeField] GameObject objetive;
    [SerializeField] float speed = 3;
    Vector3[] path;
    int pathIndex = 0;
    CharacterController controller;
    bool hasAnObjetive = false;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                objetive.transform.position = hit.point;
                path = GetComponent<AStar>().getPath(transform, objetive.transform);
                hasAnObjetive = true;
                pathIndex = 0;
            }
        }

        if(hasAnObjetive && pathIndex < path.Length)
            Move();

    }

    private void Move()
    {
        Vector3 direction = path[pathIndex] - transform.position;
        if (direction.magnitude < 0.05f)
            pathIndex++;

        controller.Move(Vector3.Normalize(direction)* 3 * Time.deltaTime);
    }
    
}
