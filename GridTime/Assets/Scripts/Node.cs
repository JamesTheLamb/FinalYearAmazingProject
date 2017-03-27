using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {

    public bool is_walkable;
    public Vector3 world_pos;

    public int grid_x;
    public int grid_y;

    public int g_cost;
    public int h_cost;

    public Node parent;

    int heap_index;

    public Node(bool _walk, Vector3 _worldPos, int _x, int _y)
    {
        is_walkable = _walk;
        world_pos = _worldPos;

        grid_x = _x;
        grid_y = _y;
    }

    public int FCost
    {
        get { return g_cost + h_cost; }
    }

    public int HeapIndex
    {
        get
        {
            return heap_index;
        }
        set
        {
            heap_index = value;
        }
    }

    public int CompareTo(Node node_compare)
    {
        int compare = FCost.CompareTo(node_compare.FCost);

        if(compare == 0)
        {
            compare = h_cost.CompareTo(node_compare.h_cost);
        }

        return -compare;
    }
 
}
