using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

	public EventSystem system;
    public GameObject obj;

    private bool button;

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && button == false)
        {
            system.SetSelectedGameObject(obj);
            button = true;
        }
    }

    private void OnDisable()
    {
        button = false;
    }
}
