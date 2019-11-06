using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStar : MonoBehaviour
{
    [SerializeField] Grid myGrid;
    Vector3[] path;

    public Vector3[] getPath(Transform start, Transform goal)
    {
        if (FindPath(goal, start))
        {
            return path;
        }
        else
            return new Vector3[0];

    }

    bool FindPath(Transform objetive, Transform start)
    {
        Node startingNode = myGrid.getNodeFromWorldPosition(start.position);
        Node objetiveNode = myGrid.getNodeFromWorldPosition(objetive.position);
        List<Node> availableNodes = new List<Node>();
        List<Node> unavailableNodes = new List<Node>();
        availableNodes.Add(startingNode);
        Node currentNode = startingNode;
        currentNode.calculateF(startingNode, objetiveNode.Pos);
        List<Node> currentNeighbords;

        while (availableNodes.Count > 0)
        {
            currentNode = availableNodes[0];
            for (int i = 1; i < availableNodes.Count; i++)
            {
                if (availableNodes[i].f <= currentNode.f)
                {
                    if (availableNodes[i].h < currentNode.h)
                    {
                        currentNode = availableNodes[i];
                    }
                }
            }

            availableNodes.Remove(currentNode);
            unavailableNodes.Add(currentNode);

            if (currentNode == objetiveNode)
            {
                while (currentNode != startingNode)
                {
                    CalculatePath(objetiveNode, startingNode);
                    return true;
                }

            }

            currentNeighbords = myGrid.GetNeighborsNodes(currentNode);

            for (int i = 0; i < currentNeighbords.Count; i++)
            {
                if (currentNeighbords[i].IsWalkable)
                {
                    if (availableNodes.Contains(currentNeighbords[i]))
                    {                    
                        Node lastNode = currentNeighbords[i].Parent;
                        currentNeighbords[i].calculateF(currentNode, objetiveNode.Pos);
                        if (currentNeighbords[i].f > currentNode.f)
                        {
                            currentNeighbords[i].calculateF(lastNode, objetiveNode.Pos);
                        }
                    }
                    else if (!unavailableNodes.Contains(currentNeighbords[i]))
                    {

                        currentNeighbords[i].calculateF(currentNode, objetiveNode.Pos);
                        availableNodes.Add(currentNeighbords[i]);
                    }
                }

            }

        }

        return false;
    }

    private void CalculatePath(Node objetiveNode, Node startingNode)
    {
        List<Node> provitionalList = new List<Node>();
        Node currentNode = objetiveNode;

        while (currentNode != startingNode)
        {
            provitionalList.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path = new Vector3[provitionalList.Count];
        for (int i = 0; i < provitionalList.Count; i++)
        {
            path[path.Length - (i + 1)] = provitionalList[i].worldPosition;
        }
    }


}
