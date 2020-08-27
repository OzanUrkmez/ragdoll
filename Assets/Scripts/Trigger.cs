using UnityEngine;

public class Trigger : MonoBehaviour
{
    private Collider collisiCollider;
    public GameObject text;
    public int texton = 5;

    void Start()
    {
        collisiCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            text.SetActive(true);
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
        text.SetActive(false);
    }
}
