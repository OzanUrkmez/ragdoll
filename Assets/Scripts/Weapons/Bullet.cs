using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 8f;
    public float bulletDuration = 2f;
    private float bulletTimer;
    void Start()
    {
        bulletTimer = bulletDuration;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        
        bulletTimer -= Time.deltaTime;
        if (bulletTimer <= 0) {
            Destroy(gameObject);
        }
    }
}
