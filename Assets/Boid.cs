using UnityEngine;

public class Boid : MonoBehaviour
{
    
    float speed = 40f;
    float rotation = 0.0f;
    
    float timer;
    Rigidbody2D rb;
   public float dist;
    
    private void Awake()
    {
        speed = Random.Range(20f, 60f);
        Destroy(gameObject,120);
        rb = GetComponent<Rigidbody2D>();
    }
    float scale = 1;
    void Update()
    {

        scale -= Time.deltaTime/120;
        transform.localScale = new Vector3(0.2f, 0.3f, 0.3f) * scale;    



        dist = Vector2.Distance(transform.position, Vector2.zero);
        if(dist > 5)
        {
            transform.up -= (transform.position - Vector3.zero)/2f * Time.deltaTime;
        }
        timer += Time.deltaTime;
        if(timer > 0.1f)
        {
            rotation = Random.Range(-0.5f, 0.5f) * Random.Range(0f,4f);
            timer = 0.0f;
        }

        rb.AddTorque(rotation);
        rb.AddForce(transform.up * speed * Time.deltaTime);
    }

    public void LookAt(Vector3 _pos)
    {
        
            transform.up -= (transform.up - _pos) / 0.6f * Time.deltaTime;

        
    }
}
