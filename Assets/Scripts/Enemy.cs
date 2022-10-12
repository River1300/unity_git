using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject eBullet;
    public GameObject ItemCoin;
    public GameObject ItemPower;
    public GameObject ItemBoom;

    public ObjectManager objectManager;

    public int enemyScore;
    public string enemyName;
    public float fireTime;
    float curTime;

    public float speed;
    public int health;

    Rigidbody rigid;

    void OnEnable()
    {
        switch(enemyName)
        {
            case "L":
                health = 40;
                break;
            case "M":
                health = 10;
                break;
            case "S":
                health = 3;
                break;
        }
    }

    void Update()
    {
        Fire();
        Reload();
    }

    void Fire()
    {
        if(curTime < fireTime) { return; }

        if(enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("eBullet");
            bullet.transform.position = transform.position;
            rigid = bullet.GetComponent<Rigidbody>();

            Vector3 dir = player.transform.position - transform.position;
            rigid.AddForce(dir.normalized * 5, ForceMode.Impulse);
        }
        else if(enemyName == "M")
        {
            GameObject bullet1 = objectManager.MakeObj("eBullet");
            bullet1.transform.position = transform.position + Vector3.right * 0.3f;
            GameObject bullet2 = objectManager.MakeObj("eBullet");
            bullet2.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody rigid1 = bullet1.GetComponent<Rigidbody>();
            Rigidbody rigid2 = bullet2.GetComponent<Rigidbody>();

            Vector3 dir1 = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dir2 = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigid1.AddForce(dir1.normalized * 4, ForceMode.Impulse);
            rigid2.AddForce(dir2.normalized * 4, ForceMode.Impulse);
        }
        curTime = 0;
    }

    void Reload()
    {
        curTime += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        if(health <= 0) { return; }
        
        health -= dmg;

        if(health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            int ran = Random.Range(0, 10);
            if(ran < 3)
            {
                Debug.Log("Not Item");
            }
            else if(ran < 5)
            {
                GameObject item = Instantiate(ItemCoin);
                item.transform.position = transform.position;
            }
            else if(ran < 8)
            {
                GameObject item = Instantiate(ItemPower);
                item.transform.position = transform.position;
            }
            else if(ran < 10)
            {
                GameObject item = Instantiate(ItemBoom);
                item.transform.position = transform.position;
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BoarderW")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if(other.gameObject.tag == "pBullet")
        {
            PlayerBullet bullet = other.gameObject.GetComponent<PlayerBullet>();
            OnHit(bullet.damage);

            other.gameObject.SetActive(false);
            other.gameObject.transform.rotation = Quaternion.identity;
        }
    }
}
