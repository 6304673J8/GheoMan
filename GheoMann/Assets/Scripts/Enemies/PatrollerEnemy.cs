using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerEnemy : Enemy
{
    public bool movingRight = true;
    public LayerMask groundLayers;

    public Transform groundCheck;
    bool isFacingRight = true;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        hp = 2;
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayers);
    }
    private void FixedUpdate()
    {
        if (hit.collider != false)
        {
            if (isFacingRight)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
        }
        else
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<HealthSystem>().health -= 1;
            hp = 0;
            //Destroy(gameObject);
            //Destroy(col.gameObject);
        }
    }
}
