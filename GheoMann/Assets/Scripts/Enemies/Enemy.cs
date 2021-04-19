using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int hp;

    public GameObject deathExplosion;
    public GameObject hpDrop;

    protected Rigidbody2D rb;

    public Vector3 center;

    public float dropRate;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    public virtual void Damage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        // Creates an explosion at the center of the enemy.
        if (deathExplosion != null)
        {
            deathExplosion = Instantiate(deathExplosion);
            deathExplosion.transform.position = transform.position + center;
            deathExplosion.transform.rotation = transform.rotation;
            deathExplosion.transform.localScale = transform.localScale;
        }

        if (Random.Range(0f, 1f) <= dropRate)
        {
            Instantiate(hpDrop, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
