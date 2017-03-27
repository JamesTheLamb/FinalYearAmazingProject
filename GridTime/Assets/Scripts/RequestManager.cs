using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RequestManager : MonoBehaviour {

    Queue<Request> request_queue = new Queue<Request>();
    Request cur_request;

    static RequestManager instance;
    Pathfinding pathfinder;

    bool is_processing;

    Vector3 end_node;

    void Awake()
    {
        instance = this;
        pathfinder = GetComponent<Pathfinding>();
        end_node = new Vector3();
    }

    public static void PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
    {

        Request new_request = new Request(start, end, callback);
        instance.end_node = end;
        instance.request_queue.Enqueue(new_request);
        instance.TryNext();
    }

    void TryNext()
    {

        if (!is_processing && request_queue.Count > 0)
        {
            cur_request = request_queue.Dequeue();
            is_processing = true;
            pathfinder.StartingPath(cur_request.start, cur_request.end);
        }
        else
        {
            
            //StopAllCoroutines();
        }
    }

    public void Finished(Vector3[] path, bool done)
    {
        cur_request.callback(path, done);

        if (path.Length > 0)
        {
            if (path[path.Length - 1] != end_node)
            {
                path[path.Length - 1] = end_node;
            }
        }

        is_processing = false;
        TryNext();

    }

    struct Request
    {
        public Vector3 start;
        public Vector3 end;
        public Action<Vector3[], bool> callback;

        public Request(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            start = _start;
            end = _end;
            callback = _callback;
        }
    }
}
