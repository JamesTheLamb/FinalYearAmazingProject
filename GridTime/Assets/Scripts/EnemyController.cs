using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

    Grid grid;

    PlayerControl player;

    GameObject closest;

    public bool found_nodes = false;

    Vector3 position;

    List<GameObject> closest_nodes;


    void Start()
    {
        grid = FindObjectOfType<Grid>();

        player = FindObjectOfType<PlayerControl>();

        position = new Vector3();

        closest_nodes = new List<GameObject>();
    }

    public void EnemyMove()
    {

        if (position != player.gameObject.transform.position)
        {
            position = player.gameObject.transform.position;

        }

        if (!found_nodes)
        {
            if (position == player.gameObject.transform.position)
            {
                FindClosestNodes(grid.tiles);

                GetComponent<EnemyUnit>().target = closest.transform;

            }
        }

    }

    void FindClosestNodes(List<GameObject> the_grid)
    {
        float distance = 30f;

        for (int i = 0; i < grid.tiles.Count; i++)
        {
            if(Vector3.Distance(gameObject.transform.position, grid.tiles[i].transform.position) < 7 &&
               Vector3.Distance(gameObject.transform.position, grid.tiles[i].transform.position) > 1.5f)
            {
                if(grid.tiles[i].GetComponent<MeshRenderer>().material.color != Color.red)
                    closest_nodes.Add(grid.tiles[i]);
            }
        }

        for(int i = 0; i < closest_nodes.Count; i++)
        {
            if(Vector3.Distance(closest_nodes[i].transform.position, position) < distance)
            {
                distance = Vector3.Distance(closest_nodes[i].transform.position, position);
                closest = closest_nodes[i];
            }
        }

        closest_nodes.Clear();

        found_nodes = true;

    }
    
}
