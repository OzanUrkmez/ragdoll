using UnityEngine;

public class DeathCollide : MonoBehaviour
{
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        void OnCollisionEnter (Collision collisionInfo)
        {
            if (collisionInfo.collider.tag == "Death")
            {
                Debug.Log("Dead"); 
            }
            
        }

    }
}
