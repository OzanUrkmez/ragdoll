using UnityEngine;

public class TriggerEnableDisable : MonoBehaviour
{
    private Collider collisiCollider;
    public GameObject enabledDisabledObject;
    public int texton = 5;

    void Start()
    {
        collisiCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            enabledDisabledObject.SetActive(true);
            Invoke("turnoff",texton);
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            collisiCollider.enabled = false;

        }
    }

    void turnoff()
    {
        enabledDisabledObject.SetActive(false);
    }
}
