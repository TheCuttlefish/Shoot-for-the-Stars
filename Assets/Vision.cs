
using UnityEngine;

public class Vision : MonoBehaviour
{
    public Boid boid;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            boid.LookAt(collision.transform.up);
        }
    }
}
