using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int actualHp;

    public Image[] healthImage;
    public Sprite fullHp;
    public Sprite emptyHp;
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < healthImage.Length; i++)
        {
            if(i < health)
            {
                healthImage[i].sprite = fullHp;
            }
            else
            {
                healthImage[i].sprite = emptyHp;
            }

            if (i < actualHp)
            {
                healthImage[i].enabled = true;
            }
            else
            {
                healthImage[i].enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health--;
        }
        if (collision.gameObject.tag == "Explosion")
        {
            health--;
        }
    }
}
