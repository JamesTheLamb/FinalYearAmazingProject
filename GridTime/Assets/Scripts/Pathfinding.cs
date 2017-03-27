using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {

    RequestManager manager;
    Grid grid;

    void Awake()
    {
        manager = GetComponent<RequestManager>();
        grid = FindObjectOfType<Grid>();
    }

    public void StartingPath(Vector3 start, Vector3 end)
    {
        StartCoroutine(Finding(start, end));

    }


    IEnumerator Finding(Vector3 start, Vector3 end)
    {

        bool done = false;

        Node s_node = grid.NodeFromWorld(start);
        Node e_node = grid.NodeFromWorld(end);


        //Starting the pathfinding algorithm
        {
            Heap<Node> open = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closed = new HashSet<Node>();

            open.Add(s_node);

            while (open.Count > 0)
            {
                Node cur_node = open.RemoveFirst();
                closed.Add(cur_node);

                if (cur_node == e_node)
                {
                    done = true;
                    break;
                }

                foreach (Node neigh in grid.GetNeighbours(cur_node))
                {
                    if (!neigh.is_walkable || closed.Contains(neigh))
                    {
                        continue;
                    }

                    int new_cost = cur_node.g_cost + GetDistanceToNode(cur_node, neigh);
                    if (new_cost < neigh.g_cost || !open.Contains(neigh))
                    {
                        neigh.g_cost = new_cost;
                        neigh.h_cost = GetDistanceToNode(neigh, e_node);
                        neigh.parent = cur_node;

                        if (!open.Contains(neigh))
                        {
                            open.Add(neigh);
                        }
                        else
                        {
                            open.UpdateItem(neigh);
                        }
                    }
                }
            }
        }
        yield return null;
        if (done)
        {
            Vector3[] waypoints = GetPath(s_node, e_node);
            manager.Finished(waypoints, done);

        }

        done = false;
    }

    Vector3[] GetPath(Node start_node, Node end_node)
    {

        List<Node> path = new List<Node>();
        Node cur_node = end_node;

        while(cur_node != start_node)
        {
            path.Add(cur_node);
            cur_node = cur_node.parent;
        }

        Vector3[] waypoints = Simplify(path);

        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] Simplify(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 old = Vector2.zero;

        for( int i = 1; i < path.Count; i++)
        {
            Vector2 new_direction = new Vector2(path[i - 1].grid_x - path[i].grid_x, path[i - 1].grid_y - path[i].grid_y);

            if(new_direction != old)
            {
                waypoints.Add(path[i].world_pos);
            }

            old = new_direction;
        }
        return waypoints.ToArray();
    }

    int GetDistanceToNode(Node node_a, Node node_b)
    {
        int d_x = Mathf.Abs(node_a.grid_x - node_b.grid_x);
        int d_y = Mathf.Abs(node_a.grid_y - node_b.grid_y);

        if(d_x > d_y)
        {
            return 14 * d_y + 10 * (d_x - d_y);
        }

        return 14 * d_y + 10 * (d_y - d_x);
    }
}
