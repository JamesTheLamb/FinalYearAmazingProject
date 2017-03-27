using UnityEngine;
using System.Collections;

public class Units : MonoBehaviour {

    TurnStateMachine state;

    public Transform target;

    float speed = 5.0f;

    Vector3[] path;

    int index;

    void Start()
    {
        state = FindObjectOfType<TurnStateMachine>();
    }

    void Update()
    {
        if (state.button_pressed && state.cur_state == TurnStateMachine.States.PLAYER)
        {
            RequestManager.PathRequest(transform.position, target.position, FoundPath);
            state.cur_state = TurnStateMachine.States.PLAYERWAIT;
            state.button_pressed = false;
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

        while(true)
        {
            if(transform.position == cur_waypoint + Vector3.up)
            {
                index++;

                if(index >= path.Length)
                {
                    yield break;
                }

                cur_waypoint = path[index];

            }

            transform.position = Vector3.MoveTowards(transform.position, cur_waypoint + Vector3.up, speed * Time.deltaTime);

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path!=null)
        {
            for(int i = index; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one);

                if(i == index)
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
