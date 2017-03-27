using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyUnit : MonoBehaviour {

    TurnStateMachine state;

    public Transform target;

    float speed = 5.0f;

    Vector3[] path;

    Animator anim;

    int index;

	// Use this for initialization
	void Start ()
    {
        state = FindObjectOfType<TurnStateMachine>();

        anim = GetComponentInChildren<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

        if(state.enemy_moved && state.cur_state == TurnStateMachine.States.ENEMY && GetComponent<EnemyController>().found_nodes)
        {
            RequestManager.PathRequest(transform.position, target.position, FoundPath);
            state.enemy_moved = false;
            GetComponent<EnemyController>().found_nodes = false;
            state.cur_state = TurnStateMachine.States.ENEMYWAIT;

        }

    }

    public void FoundPath(Vector3[] _path, bool _done)
    {
        if(_done)
        {
            path = _path;
            index = 0;
            StopCoroutine("Follow");
            StartCoroutine("Follow");
        }
    }

    IEnumerator Follow()
    {
        Vector3 cur_waypoint = path[0];

        while (true)
        {
            if (transform.position == cur_waypoint)
            {
                index++;

                if (index >= path.Length)
                {
                    anim.Play("Idle");
                    yield break;
                }

                cur_waypoint = path[index];
                if (cur_waypoint + Vector3.up == FindObjectOfType<PlayerHealth>().transform.position)
                {
                    yield break;
                }

            }
            anim.Play("Running");
            transform.position = Vector3.MoveTowards(transform.position, cur_waypoint, speed * Time.deltaTime);

            yield return null;
        }


    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = index; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == index)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
