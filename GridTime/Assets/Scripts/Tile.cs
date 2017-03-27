using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tile : MonoBehaviour {

    PlayerControl player;

    public bool can_walk = true;
    public bool is_turn;

    public bool is_highlighted = false;

    void Start()
    {
        player = FindObjectOfType<PlayerControl>();

    }

	void Update()
    {

        ColourTheGrid(player.gameObject);
    }

    void ColourTheGrid(GameObject entity)
    {
        if(can_walk && Vector3.Distance(entity.transform.position, transform.position) <= 12 && is_highlighted)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (can_walk && Vector3.Distance(entity.transform.position, transform.position) > 12 && !is_highlighted ||
                 can_walk && Vector3.Distance(entity.transform.position, transform.position) < 3 && !is_highlighted)
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else if (can_walk && Vector3.Distance(entity.transform.position, transform.position) <= 12 && !is_highlighted &&
                 Vector3.Distance(entity.transform.position, transform.position) >= 3)
        {
            GetComponent<MeshRenderer>().material.color = Color.cyan;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }


}
