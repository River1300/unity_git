using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isTouchTop;
    bool isTouchBottom;
    bool isTouchLeft;
    bool isTouchRight;

    public GameObject pBullet;
    public GameObject boombEffect;
    public GameManager gameManager;
    public ObjectManager objectManager;
    Rigidbody rigid;

    public bool isHit;
    public bool isBoombTime;
    public int life;
    public int bestScore;
    public int score;
    public int power;
    public int maxPower;
    public int boomb;
    public int maxBoomb;
    public float speed;
    public float fireTime;
    float curTime = 0;
    Vector3 dir;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        bestScore = PlayerPrefs.GetInt("Best Score", 0);
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
        Boomb();
        BestScore();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if((h == 1 && isTouchRight) || (h == -1 && isTouchLeft)) { h = 0; }
        float v = Input.GetAxisRaw("Vertical");
        if((v == 1 && isTouchTop) || (v == -1 && isTouchBottom)) { v = 0; }

        dir = new Vector3(h, v, 0);
        
        transform.position += dir * speed * Time.deltaTime;

        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Fire()
    {
        if(!Input.GetButton("Fire1")) { return; }

        if(curTime < fireTime) { return; }

        switch(power)
        {
            case 1:
                GameObject bullet1 = objectManager.MakeObj("pBullet");
                bullet1.transform.position = transform.position;
                rigid = bullet1.GetComponent<Rigidbody>();
                rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
                break;
            case 2:
                int shot = 5;
                for(int i = 0; i < shot; i++)
                {
                    GameObject bullet2 = objectManager.MakeObj("pBullet");
                    bullet2.transform.position = 
                        (transform.position + Vector3.left * 0.6f) + Vector3.right * (i * 0.3f);
                    rigid = bullet2.GetComponent<Rigidbody>();
                    rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
                }
                break;
            case 3:
                int round = 25;
                for(int i = 0; i < round; i++)
                {
                    GameObject bullet3 = objectManager.MakeObj("pBullet");
                    bullet3.transform.position = transform.position;
                    bullet3.transform.rotation = Quaternion.identity;
                    rigid = bullet3.GetComponent<Rigidbody>();
                    Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / round),
                                                Mathf.Sin(Mathf.PI * 2 * i / round));
                    rigid.AddForce(dirVec.normalized * 10, ForceMode.Impulse);
                    Vector3 rotVec = Vector3.forward * 360 * i / round + Vector3.forward * 90;
                    bullet3.transform.Rotate(rotVec);
                }
                break;
        }
        curTime = 0;
    }

    void Reload()
    {
        curTime += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Boarder")
        {
            switch(other.gameObject.name)
            {
            case "Top":
                isTouchTop = true;
                break;
            case "Bottom":
                isTouchBottom = true;
                break;
            case "Left":
                isTouchLeft = true;
                break;
            case "Right":
                isTouchRight = true;
                break;
            }
        }
        else if(other.gameObject.tag == "eBullet")
        {
            if(isHit) { return; }
            isHit = true;

            life--;
            gameManager.CoutLife(life);

            if(life <= 0)
            {
                gameManager.GameOverSet();
            }
            else
            {
                gameManager.RespawnCall();
            }
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "Item")
        {
            Item item = other.gameObject.GetComponent<Item>();
            
            switch(item.name)
            {
                case "C":
                    score += 1000;
                    break;

                case "P":
                    if(power == maxPower)
                    {
                        score += 500;
                    }
                    else
                    {
                        power++;
                    }
                    break;

                case "B":
                    if(boomb == maxBoomb)
                    {
                        score += 500;
                    }
                    else
                    {
                        boomb++;
                        gameManager.CountBoom(boomb);
                    }
                    break;
            }
            other.gameObject.SetActive(false);
        }
    }

    void BestScore()
    {
        if(bestScore < score) 
        {
            bestScore = score;
            PlayerPrefs.SetInt("Best Score", bestScore);
        }
    }

    void Boomb()
    {
        if(!Input.GetButton("Fire2")) { return; }
        if(isBoombTime) { return; }
        if(boomb == 0) { return; }

        boomb--;
        gameManager.CountBoom(boomb);
        isBoombTime = true;

        boombEffect.SetActive(true);
        Invoke("OffBoombEffect", 2.5f);

        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for(int i = 0; i < enemiesL.Length; i++)
        {
            if(enemiesL[i].activeSelf){
                Enemy enemyLogic = enemiesL[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for(int i = 0; i < enemiesM.Length; i++)
        {
            if(enemiesM[i].activeSelf){
                Enemy enemyLogic = enemiesM[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for(int i = 0; i < enemiesS.Length; i++)
        {
            if(enemiesS[i].activeSelf){
                Enemy enemyLogic = enemiesS[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        GameObject[] bullets = objectManager.GetPool("eBullet");
        for(int i = 0; i < bullets.Length; i++)
        {
            if(bullets[i].activeSelf)
            {
                bullets[i].SetActive(false);
            }
        }
    }

    void OffBoombEffect()
    {
        boombEffect.SetActive(false);
        isBoombTime = false;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Boarder")
        {
            switch(other.gameObject.name)
            {
            case "Top":
                isTouchTop = false;
                break;
            case "Bottom":
                isTouchBottom = false;
                break;
            case "Left":
                isTouchLeft = false;
                break;
            case "Right":
                isTouchRight = false;
                break;
            }
        }
    }
}
