using UnityEngine;
using System.Collections;

public class HealthFacesUs : MonoBehaviour {

    Camera cam;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

	void Update()
    {
        transform.LookAt(cam.transform);
    }

}
