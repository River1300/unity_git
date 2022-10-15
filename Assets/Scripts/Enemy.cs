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

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    Rigidbody rigid;

    void OnEnable()
    {
        switch(enemyName)
        {
            case "B":
                health = 500;
                Invoke("Stop", 3);
                break;
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

    void Stop()
    {
        if(!gameObject.activeSelf) { return; }
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        //CancelInvoke();
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch(patternIndex)
        {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireForward()
    {
        if(health <= 0) { return; }

        GameObject bulletR = objectManager.MakeObj("bBullet");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("bBullet");
        bulletRR.transform.position = transform.position + Vector3.right * 0.65f;
        GameObject bulletL = objectManager.MakeObj("bBullet");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("bBullet");
        bulletLL.transform.position = transform.position + Vector3.left * 0.65f;

        Rigidbody rigid1 = bulletR.GetComponent<Rigidbody>();
        Rigidbody rigid2 = bulletRR.GetComponent<Rigidbody>();
        Rigidbody rigid3 = bulletL.GetComponent<Rigidbody>();
        Rigidbody rigid4 = bulletLL.GetComponent<Rigidbody>();

        rigid1.AddForce(Vector2.down * 6, ForceMode.Impulse);
        rigid2.AddForce(Vector2.down * 6, ForceMode.Impulse);
        rigid3.AddForce(Vector2.down * 6, ForceMode.Impulse);
        rigid4.AddForce(Vector2.down * 6, ForceMode.Impulse);

        curPatternCount++;
        if(curPatternCount < maxPatternCount[patternIndex]) 
        {
            Invoke("FireForward", 1);
        }
        else 
        { 
            Invoke("Think", 3);
        }
    }
    void FireShot()
    {
        if(health <= 0) { return; }

        for(int i = 0; i < 5; i++){
            GameObject bulletS = objectManager.MakeObj("bBullet");
            bulletS.transform.position = transform.position;
            rigid = bulletS.GetComponent<Rigidbody>();

            Vector3 dir = player.transform.position - transform.position;
            Vector3 ranDir = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f), 0);
            dir += ranDir;
            rigid.AddForce(dir.normalized * 5, ForceMode.Impulse);
        }

        curPatternCount++;
        if(curPatternCount < maxPatternCount[patternIndex]) 
        { 
            Invoke("FireShot", 1.55f);
        }
        else 
        { 
            Invoke("Think", 3);
        }
    }
    void FireArc()
    {
        if(health <= 0) { return; }

        GameObject bulletF = objectManager.MakeObj("bBullet");
        bulletF.transform.position = transform.position;
        bulletF.transform.rotation = Quaternion.identity;
        rigid = bulletF.GetComponent<Rigidbody>();

        Vector3 dirF = new Vector3(Mathf.Cos
                        (Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1, 0);
        rigid.AddForce(dirF.normalized * 5, ForceMode.Impulse);

        curPatternCount++;
        if(curPatternCount < maxPatternCount[patternIndex]) 
        { 
            Invoke("FireArc", 0.15f);
        }
        else 
        { 
            Invoke("Think", 3);
        }
    }
    void FireAround()
    {
        if(health <= 0) { return; }

        int roundB = 50;
        int roundBB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundB : roundBB;

        for(int i = 0; i < roundNum; i++){
            GameObject bulletO = objectManager.MakeObj("bBullet");
            bulletO.transform.position = transform.position;
            bulletO.transform.rotation = Quaternion.identity;
            rigid = bulletO.GetComponent<Rigidbody>();

            Vector3 dirO = new Vector3(Mathf.Sin(Mathf.PI * 2 * i / roundNum), 
                                        Mathf.Cos(Mathf.PI * 2 * i / roundNum), 0);
            rigid.AddForce(dirO.normalized * 3, ForceMode.Impulse);
        }

        curPatternCount++;
        if(curPatternCount < maxPatternCount[patternIndex]) 
        { 
            Invoke("FireAround", 2f);
        }
        else 
        {
            Invoke("Think", 3);
        }
    }

    void Update()
    {
        if(enemyName == "B") { return; }
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

            int ran = enemyName == "B" ? 0 : Random.Range(0, 10);
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

            if(enemyName == "B") { objectManager.DeleteAll("BOSS"); }
            CancelInvoke();
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BoarderW" && enemyName != "B")
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
