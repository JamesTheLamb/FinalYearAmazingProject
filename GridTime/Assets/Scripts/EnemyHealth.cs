using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    EntityHealth entity;

    GameObject player;


    void Start()
    {
        entity = new EntityHealth();
        entity.Health = 50;

        player = FindObjectOfType<PlayerHealth>().gameObject;

    }

    void Update()
    {
        

        if(entity.Health <= 0 && !entity.IsDead)
        {
            entity.IsDead = true;
        }
    }

    public void MeleeAttack()
    {
        player.GetComponent<PlayerHealth>().GetEntity.TakeDamage(20);
    }

    public void RangedAttack()
    {
        player.GetComponent<PlayerHealth>().GetEntity.TakeDamage(10);
    }

    public EntityHealth GetEntity
    {
        get
        {
            return entity;
        }
    }

   
}
