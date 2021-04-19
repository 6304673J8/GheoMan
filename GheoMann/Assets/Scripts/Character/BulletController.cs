using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rb;
    public float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroySelf", .6f);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(baseSpeed, 0);
        //transform.position += transform.right * baseSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.rigidbody.GetComponent<Enemy>();
            if (enemy.hp <= 0)
            {
                DestroySelf();
            }
        }
        if (collision.gameObject.tag == "Scenario")
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
