using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public GameObject plane_prefab;
    public List<GameObject> tiles;

    public LayerMask unwalkable_mask;
    public Vector2 grid_size;
    public float node_radius;

    Node[,] grid;

    float node_diameter;
    int grid_x, grid_y;

    void Awake()
    {
        node_diameter = node_radius * 2;

        grid_x = Mathf.RoundToInt(grid_size.x / node_diameter);
        grid_y = Mathf.RoundToInt(grid_size.y / node_diameter);

        tiles = new List<GameObject>();

        CreateGrid();


    }

    public int MaxSize
    {
        get
        {
            return grid_x * grid_y;
        }
    }


    void CreateGrid()
    {
        grid = new Node[grid_x, grid_y];

        Vector3 BL = transform.position - Vector3.right * grid_size.x / 2 - Vector3.forward * grid_size.y / 2;

        for (int x = 0; x < grid_x; x++)
        {
            for (int y = 0; y < grid_y; y++)
            {
                Vector3 world_point = BL + Vector3.right * (x * node_diameter + node_radius) + Vector3.forward * (y * node_diameter + node_radius);
                bool walk = !(Physics.CheckSphere(world_point, node_radius, unwalkable_mask));
                grid[x, y] = new Node(walk, world_point, x, y);

                GameObject go = Instantiate(plane_prefab);
                go.transform.position = new Vector3(x-15f, 0, y-15f);
                go.GetComponent<Tile>().can_walk = walk;
                tiles.Add(go);
            }
        }
    }

    public Node[,] GetGrid()
    {
        return grid;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neigh = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int check_x = node.grid_x + x;
                int check_y = node.grid_y + y;

                if(check_x >= 0 && check_x < grid_x && check_y >= 0 && check_y < grid_y)
                {
                    neigh.Add(grid[check_x, check_y]);
                }
            }

        }

        return neigh;
    }

    public Node NodeFromWorld(Vector3 worldPos)
    {

        float percent_x = (worldPos.x + grid_size.x / 2) / grid_size.x;
        float percent_y= (worldPos.z + grid_size.y / 2) / grid_size.y;

        percent_x = Mathf.Clamp01(percent_x);
        percent_y = Mathf.Clamp01(percent_y);

        int x = Mathf.RoundToInt((grid_x) * percent_x);
        int y = Mathf.RoundToInt((grid_y) * percent_y);

        return grid[x, y];
    }
    
}
