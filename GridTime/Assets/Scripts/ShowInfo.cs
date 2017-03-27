using UnityEngine;
using System.Collections;

public class ShowInfo : MonoBehaviour {

    Transform previous;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.GetComponent<EnemyHealth>() && hit.transform.GetComponent<MeshRenderer>().enabled)
                {
                    if (previous != null)
                        previous.GetChild(0).gameObject.SetActive(false);

                    hit.transform.GetChild(0).gameObject.SetActive(true);
                    previous = hit.transform;
                }
                else if (hit.transform.GetComponent<PlayerHealth>())
                {
                    if (previous != null)
                        previous.GetChild(0).gameObject.SetActive(false);

                    hit.transform.GetChild(0).gameObject.SetActive(true);
                    previous = hit.transform;
                }
                else
                {
                    if (previous != null)
                    {
                        previous.GetChild(0).gameObject.SetActive(false);
                        previous = null;
                    }
                }
            }
        }
    }

}
