using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChooseWhoToAttack : MonoBehaviour {

    PlayerHealth player;

    EnemyHealth[] enemies;

    List<EnemyHealth> visible_enemies;

    public bool choosing = false;

    public GameObject buttons;
    public GameObject next_button;

    public Text text;

    Camera main_cam;

    int counter = 0;

    TurnStateMachine turn;

    void Start()
    {
        turn = FindObjectOfType<TurnStateMachine>();

        main_cam = FindObjectOfType<Camera>();

        player = FindObjectOfType<PlayerHealth>();

        enemies = FindObjectsOfType<EnemyHealth>();
        
        visible_enemies = new List<EnemyHealth>();
    }

    void Update()
    {
        if(choosing)
        {

            main_cam.transform.position = Vector3.Lerp(main_cam.transform.position, player.transform.position + (Vector3.up * 2), 5 * Time.deltaTime);


            main_cam.transform.LookAt(visible_enemies[counter].gameObject.transform);

            text.text = visible_enemies[counter].gameObject.name + ". Health: " + visible_enemies[counter].GetEntity.Health;

            if(player.EnemyOfPlayer != visible_enemies[counter].gameObject)
                player.EnemyOfPlayer = visible_enemies[counter].gameObject;

        }
        else
        {
            if(next_button.activeInHierarchy)
            {
                buttons.SetActive(true);
                next_button.SetActive(false);

                turn.cur_state = TurnStateMachine.States.PLAYERWAIT;

            }
        }

        
    }


    public void WhoVisible()
    {
        if (visible_enemies.Count > 0)
            visible_enemies.Clear();

        for(int i = 0; i < enemies.Length; i++)
        {
            Ray ray = new Ray(player.transform.position, enemies[i].transform.position - player.transform.position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.name == enemies[i].transform.name)
                {
                    visible_enemies.Add(enemies[i]);
                }
            }

        }


        buttons.SetActive(false);
        next_button.SetActive(true);

        choosing = true;
    }

    public void AttackThisOne()
    {
        player.EnemyOfPlayer = visible_enemies[counter].gameObject;
        player.FarAttack();
        choosing = false;
        counter = 0;
    }

    public void MeleeAttackThisOne()
    {
        player.EnemyOfPlayer = visible_enemies[counter].gameObject;
        player.MeleeAttack();
        choosing = false;
        counter = 0;
    }

    public void NextClicked()
    {

        if (visible_enemies.Count >= 0)
        {
            if (counter == visible_enemies.Count - 1)
                counter = 0;
            else
                counter++;
        }

        
    }

    public List<EnemyHealth> GetVisible
    {
        get
        {
            return visible_enemies;
        }
    }


}
