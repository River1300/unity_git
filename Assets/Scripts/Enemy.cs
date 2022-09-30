using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;

    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.down * speed;
    }

    void OnHit(int dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BoarderW")
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "pBullet")
        {
            PlayerBullet bullet = other.gameObject.GetComponent<PlayerBullet>();
            OnHit(bullet.damage);

            Destroy(other.gameObject);
        }
    }
}
