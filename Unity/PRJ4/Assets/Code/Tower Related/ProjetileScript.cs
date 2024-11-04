using UnityEngine;

public class ProjetileScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public int bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<MyEnemyScript>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
