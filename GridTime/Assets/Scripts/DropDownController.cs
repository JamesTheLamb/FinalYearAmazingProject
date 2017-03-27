using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DropDownController : MonoBehaviour
{


    public Dropdown drop_;

    GameObject player_;

    GameObject[] enemies_;

    List<string>[] names_;

    bool is_dropdown = false;

    public GameObject selected_enemy;

    void Start()
    {
        player_ = FindObjectOfType<PlayerHealth>().gameObject;

        enemies_ = new GameObject[FindObjectsOfType<EnemyHealth>().Length];
        names_ = new List<string>[FindObjectsOfType<EnemyHealth>().Length];

        for(int i = 0; i < names_.Length; i++)
        {
            names_[i] = new List<string>();
        }


        for (int i = 0; i < enemies_.Length; i++)
        {
            enemies_[i] = FindObjectsOfType<EnemyHealth>()[i].gameObject;
            names_[i].Add(FindObjectsOfType<EnemyHealth>()[i].gameObject.name);
        }

        drop_.ClearOptions();
    }

    void Update()
    {
        GetSelectedEnemy();

        if (drop_.options.Count == 0)
            drop_.ClearOptions();

        for (int i = 0; i < enemies_.Length; i++)
        {
            Ray ray = new Ray(player_.transform.position + new Vector3(0, 0.3f, 0), (enemies_[i].transform.position - player_.transform.position) + new Vector3(0, 0.3f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject == enemies_[i])
                {
                    for (int j = 0; j < drop_.options.Count; j++)
                    {
                        if(drop_.options[j].text == enemies_[i].name)
                        {
                            is_dropdown = true;
                            continue;
                        }
                    }

                    if(!is_dropdown)
                    {
                        drop_.AddOptions(names_[i]);
                        is_dropdown = false;
                    }
                }
                else
                {
                    for (int j = 0; j < drop_.options.Count; j++)
                    {
                        if(drop_.options[j].text == enemies_[i].name)
                        {
                            drop_.options.RemoveAt(j);
                        }
                    }
                }
            }

            is_dropdown = false;
        }
    }

    void GetSelectedEnemy()
    {
        if (drop_.options.Count == 0)
        {
            selected_enemy = null;
            return;
        }

        for (int i = 0; i < enemies_.Length; i++)
        {
            if(drop_.options[drop_.value].text == enemies_[i].name)
            {
                selected_enemy = enemies_[i];
                
                break;
            }
        }
    }
}