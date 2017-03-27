using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public Button close_button;
    public Button far_button;

    Animator anim;
    bool anim_go = false;

    [SerializeField]
    EntityHealth entity;

    EnemyHealth[] enemy;

    public GameObject projectile;
    public GameObject ax;
    GameObject ax_;
    GameObject go;

    bool f_attacked = false;
    bool has_instantiated = false;

    float cur_time = 0f;
    float timer = 1f;


    GameObject the_enemy;

    void Start()
    {

        entity = new EntityHealth();
        entity.Health = 100;

        enemy = FindObjectsOfType<EnemyHealth>();

        go = Instantiate(projectile, transform.position - new Vector3(0, 0.4f, 0), Quaternion.identity) as GameObject;

        ax_ = Instantiate(ax, transform.position, Quaternion.identity) as GameObject;

        anim = ax_.GetComponent<Animator>();

    }


    void Update()
    {

        #region FarAttack
        if (f_attacked)
        {
            if (the_enemy != null)
            {
                float dist = Vector3.Distance(the_enemy.transform.position, go.transform.position);

                if (dist >= 0.6f)
                {
                    go.transform.LookAt(the_enemy.transform);
                    go.transform.position += (go.transform.forward * Time.deltaTime) * 4;
                }
                else
                {
                    f_attacked = false;
                    the_enemy.GetComponent<EnemyHealth>().GetEntity.TakeDamage(10);
                }
            }
        }
        else
        {
            go.transform.position = transform.position;
        }
        #endregion

        #region MeleeAttack
        ax_.transform.position = transform.position + new Vector3(0, 0.75f, 0);

        if(anim_go)
        {
            if(!ax_.GetComponent<MeshRenderer>().enabled)
            {
                ax_.GetComponent<MeshRenderer>().enabled = true;
                ax_.transform.Find("Cube").GetComponent<Renderer>().enabled = true;

            }

            anim.Play("AxAnim");

            if (cur_time >= timer)
            {
                anim_go = false;
                cur_time = 0f;
            }
            else
                cur_time += Time.deltaTime;
        }
        else
        {
            if(ax_.GetComponent<MeshRenderer>().enabled)
            {
                
                ax_.GetComponentInChildren<Renderer>().enabled = false;
                ax_.transform.Find("Cube").GetComponent<Renderer>().enabled = false;
            }
        }

        #endregion

        #region FarButton
        if (the_enemy != null)
        {
            Ray ray = new Ray(transform.position + new Vector3(0, 0.3f, 0), (the_enemy.transform.position - transform.position) + new Vector3(0, 0.3f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform == the_enemy.transform)
                {
                    if (!far_button.IsInteractable())
                    {
                        far_button.interactable = true;
                    }

                }
                else
                {
                    if (far_button.IsInteractable())
                    {
                        far_button.interactable = false;
                    }

                }
            }
        }
        else
        {
            if(far_button.IsInteractable())
            {
                far_button.interactable = false;
            }
        }
        #endregion

        #region MeleeButton
        if (the_enemy != null)
        {
            if (Vector3.Distance(the_enemy.transform.position, transform.position) < entity.close_range)
            {
                if (!close_button.IsInteractable())
                {
                    close_button.interactable = true;
                }
            }
            else
            {
                if (close_button.IsInteractable())
                {
                    close_button.interactable = false;
                }

            }
        }
        else
        {
            if(close_button.IsInteractable())
            {
                close_button.interactable = false;
            }
        }
        #endregion

        

        if (entity.Health <= 0 && !entity.IsDead)
        {
            entity.IsDead = true;
        }
    }

    public EntityHealth GetEntity
    {
        get
        {
            return entity;
        }
    }

    public void MeleeAttack()
    {
        if(Vector3.Distance(the_enemy.transform.position, transform.position) < entity.close_range)
        {
            the_enemy.GetComponent<EnemyHealth>().GetEntity.TakeDamage(20);
            anim_go = true;
        }
        
    }
    public void FarAttack()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.3f, 0), (the_enemy.transform.position - transform.position) + new Vector3(0, 0.3f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.GetComponent<EnemyHealth>())
            {
                f_attacked = true;
            }
        }
    }

    public GameObject EnemyOfPlayer
    {
        get
        {
            return the_enemy;
        }
        set
        {
            the_enemy = value;
        }
    }


}
