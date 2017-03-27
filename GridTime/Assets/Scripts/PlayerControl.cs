using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    GameObject previous;

    EnemyHealth[] enemies;

    Grid grid;

    int index;

    void Start()
    {
        grid = FindObjectOfType<Grid>();

        enemies = FindObjectsOfType<EnemyHealth>();
    }

	// Update is called once per frame
	public void PlayerGetNode () {

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.gameObject.GetComponent<Tile>())
                    {

                        if (Vector3.Distance(hit.transform.gameObject.transform.position, transform.position) <= 12 & hit.transform.gameObject.GetComponent<MeshRenderer>().material.color == Color.cyan)
                        {

                            index = grid.tiles.IndexOf(hit.transform.gameObject);

                            for (int i = 0; i < enemies.Length; i++)
                            {
                                if (grid.tiles[index].transform.position != enemies[i].gameObject.transform.position + new Vector3(0, 1, 0))
                                {

                                }
                                else if (grid.tiles[index].transform.position == enemies[i].gameObject.transform.position + new Vector3(0, 1, 0))
                                {
                                    return;
                                }
                            }

                            if (previous != null)
                            {
                                previous.GetComponent<Tile>().is_highlighted = false;
                            }

                            grid.tiles[index].GetComponent<Tile>().is_highlighted = true;

                            previous = grid.tiles[index];

                            GetComponent<Units>().target = grid.tiles[index].transform;

                        }
                        else
                        {
                            //Debug.Log("Got: " + hit.transform.gameObject.name);
                        }
                    }

                }
            }
        }


    }


}
