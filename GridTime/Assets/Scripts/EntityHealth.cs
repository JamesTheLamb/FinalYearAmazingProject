using UnityEngine;
using System.Collections;

[System.Serializable]
public class EntityHealth {

    [SerializeField]
    int hp;

    public float close_range = 3.5f;
    public float far_range = 8f;

    bool is_dead = false;

    public int Health
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return is_dead;
        }
        set
        {
            is_dead = value;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }
}
