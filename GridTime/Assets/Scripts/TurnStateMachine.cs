using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TurnStateMachine : MonoBehaviour {

    Camera cam;
    public enum States
    {
        START,
        PLAYER,
        PLAYERFAR,
        PLAYERCLOSE,
        PLAYERWAIT,
        ENEMY,
        ENEMYFAR,
        ENEMYCLOSE,
        ENEMYWAIT,
        LOSE,
        WIN
    }

    public bool button_pressed = false;
    public bool enemy_moved = false;
    bool has_attacked = false;
    bool has_close_attacked = false;
    bool has_passed = false;
    bool player_second = false;
    bool lerp_out = false;
    bool turned = true;

    public States cur_state;

    GameObject player;

    PlayerHealth p_attacks;

    GameObject[] enemy;
    EntityHealth[] e_entity;

    float timer = 5f;

    float cur_time = 0f;

    public Text text;
    public GameObject buttons;
    public GameObject return_button;

    Vector3 offset; 

    float cam_max = 1f;
    float cam_min = 0.5f;
    float cam_dist = 1f;
    float scroll_speed = 1f;

    int counter;
    int cam_counter = 0;
    int lefty = 2;

    ChooseWhoToAttack choose;


	// Use this for initialization
	void Start () {

        choose = FindObjectOfType<ChooseWhoToAttack>();

        cam = FindObjectOfType<Camera>();

        cur_state = States.PLAYER;

        player = FindObjectOfType<PlayerControl>().gameObject;
        p_attacks = player.GetComponent<PlayerHealth>();

        enemy = new GameObject[FindObjectsOfType<EnemyController>().Length];
        e_entity = new EntityHealth[FindObjectsOfType<EnemyController>().Length];

        for (int i = 0; i < FindObjectsOfType<EnemyController>().Length; i++)
        {
            enemy[i] = FindObjectsOfType<EnemyController>()[i].gameObject;
            e_entity[i] = FindObjectsOfType<EnemyHealth>()[i].GetEntity;
        }

        counter = 0;

        offset = player.transform.position + new Vector3(-3, 10, -3);

    }
	
	// Update is called once per frame
	void Update () {

        if (p_attacks.GetEntity.Health <= 0 && cur_state != States.LOSE)
        {
            cur_state = States.LOSE;
            return;
        }

        if (!choose.choosing)
        {
            lerp_out = true;

            if(turned)
                ApplyCameraTransformation(player, lefty);
        }

        for(int i = 0; i < enemy.Length; i++)
        {
            if(i == enemy.Length)
            {
                if(e_entity[i].IsDead)
                {
                    cur_state = States.WIN;
                }
            }

            if (e_entity[i].IsDead)
                continue;
            else
                break;
        }
        

        switch (cur_state)
        {
            case (States.START):
                break;

            case (States.PLAYER):

                if(!buttons.activeInHierarchy)
                {
                    buttons.SetActive(true);
                }

                if(has_attacked)
                {
                    choose.WhoVisible();
                    cur_state = States.PLAYERFAR;
                }

                if(has_close_attacked)
                {
                    p_attacks.MeleeAttack();
                    cur_state = States.PLAYERWAIT;
                }

                if(has_passed)
                {
                    cur_state = States.PLAYERWAIT;
                }

                player.GetComponent<PlayerControl>().PlayerGetNode();

                if (!player_second)
                    text.text = "Player's Turn 1/2";
                else
                    text.text = "Player's Turn 2/2";

                break;

            case (States.PLAYERFAR):
                break;

            case (States.PLAYERCLOSE):
                break;

            case (States.PLAYERWAIT):

                if(buttons.activeInHierarchy)
                {
                    buttons.SetActive(false);
                }

                if (has_attacked)
                    has_attacked = false;

                if (has_close_attacked)
                    has_close_attacked = false;

                if (has_passed)
                    has_passed = false;

                if(!player_second)
                    text.text = "Player's Turn 1/2 Ending In: " + Mathf.RoundToInt((5f - cur_time)).ToString();
                else
                    text.text = "Player's Turn 2/2 Ending In: " + Mathf.RoundToInt((5f - cur_time)).ToString();

                if (cur_time >= timer)
                {
                    cur_time = 0f;
                    counter = 0;

                    if (!player_second)
                    {
                        if (!player_second)
                        {
                            player_second = true;
                        }

                        cur_state = States.PLAYER;
                    }
                    else
                        cur_state = States.ENEMY;

                }
                else
                    cur_time += Time.deltaTime;

                break;

            case (States.ENEMY):

                float dist = Vector3.Distance(player.transform.position, enemy[counter].transform.position);

                Ray ray = new Ray(enemy[counter].transform.position, player.transform.position - enemy[counter].transform.position);
                RaycastHit hit;

                if (cur_time >= timer)
                {
                    cur_time = 0f;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        if (dist > e_entity[counter].far_range)
                        {
                            enemy_moved = true;
                            enemy[counter].GetComponent<EnemyController>().EnemyMove();
                            Debug.Log("enemy moved");
                        }
                        else if (dist <= e_entity[counter].far_range && dist > e_entity[counter].close_range &&
                                 hit.transform.GetComponent<PlayerHealth>())
                        {
                            has_attacked = true;
                            enemy[counter].GetComponent<EnemyHealth>().RangedAttack();
                            cur_state = States.ENEMYWAIT;
                            Debug.Log("enemy sniped");
                        }
                        else if (dist <= e_entity[counter].close_range &&
                                 hit.transform.GetComponent<PlayerHealth>())
                        {
                            has_close_attacked = true;
                            enemy[counter].GetComponent<EnemyHealth>().MeleeAttack();
                            cur_state = States.ENEMYWAIT;
                            Debug.Log("enemy melee");


                        }
                        else
                        {
                            enemy_moved = true;
                            enemy[counter].GetComponent<EnemyController>().EnemyMove();
                            Debug.Log("enemy moved");

                        }
                    }
                }
                else
                    cur_time += Time.deltaTime;

                text.text = "Enemy's Turn";


                break;

            case (States.ENEMYFAR):
                break;

            case (States.ENEMYCLOSE):
                break;

            case (States.ENEMYWAIT):
                text.text = "Enemy's Turn Ending In: " + Mathf.RoundToInt((5f - cur_time)).ToString();

                if (has_attacked)
                    has_attacked = false;

                if (has_close_attacked)
                    has_close_attacked = false;

                if (cur_time >= timer)
                {
                    cur_time = 0f;

                    if(counter < enemy.Length - 1)
                    {
                        counter++;
                        cur_state = States.ENEMY;
                    }
                    else
                    {
                        player_second = false;
                        cur_state = States.PLAYER;
                    }
                }
                else
                    cur_time += Time.deltaTime;


                break;

            case (States.LOSE):

                text.text = "You died! Too bad...";
                buttons.SetActive(false);
                return_button.SetActive(true);

                break;

            case (States.WIN):
                text.text = "Congratulations, you win!";

                buttons.SetActive(false);
                return_button.SetActive(true);

                break;
        }
	}

    public void ApplyCameraTransformation(GameObject entity, int turn)
    {
        cam_dist += -Input.GetAxis("Mouse ScrollWheel") * scroll_speed;
        cam_dist = Mathf.Clamp(cam_dist, cam_min, cam_max);

        //if(lerp_out & cam.transform.position != entity.transform.position + offsets[cam_counter] * cam_dist)
        //{
        //    Debug.Log("Lerp out");
        //    Vector3.Lerp(cam.transform.position, entity.transform.position + offsets[cam_counter] * cam_dist, 5 * Time.deltaTime);
        //    lerp_out = false;
        //}
        //else
        //{
        //    cam.gameObject.transform.position = entity.transform.position + offsets[cam_counter] * cam_dist;

        //}

        cam.gameObject.transform.position = entity.transform.position + offset * cam_dist;

        if(lefty == 0)
        {
            cam.gameObject.transform.RotateAround(player.transform.position, Vector3.up, 90);
        }
        else
        {
            cam.gameObject.transform.RotateAround(player.transform.position, Vector3.up, -90);
        }

        cam.transform.LookAt(entity.transform.position);

        turned = false;
        
    }

    public void CamCounterLeft()
    {
        lefty = 0;
        turned = true;
        
    }

    public void CamCounterRight()
    {
        lefty = 1;
        turned = true;
    }

    public void SwitchTurn()
    {
        if (!button_pressed)
        {
            button_pressed = true;
        }
    }

    public void Attacked()
    {
        if (!has_attacked)
            has_attacked = true;
    }

    public void MeleeAttack()
    {
        if (!has_close_attacked)
            has_close_attacked = true;
    }

    public void PassedTurn()
    {
        if (!has_passed)
            has_passed = true;
    }
}
