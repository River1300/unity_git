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
    Rigidbody rigid;

    public int power;
    public float speed;
    public float fireTime;
    float curTime = 0;
    Vector3 dir;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
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
                GameObject bullet1 = Instantiate(pBullet);
                bullet1.transform.position = transform.position;
                rigid = bullet1.GetComponent<Rigidbody>();
                rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
                break;
            case 2:
                int shot = 5;
                for(int i = 0; i < shot; i++)
                {
                    GameObject bullet2 = Instantiate(pBullet);
                    bullet2.transform.position = 
                        (transform.position + Vector3.left * 0.6f) + Vector3.right * (i * 0.3f);
                    rigid = bullet2.GetComponent<Rigidbody>();
                    rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
                }
                break;
            case 3:
                int round = 50;
                for(int i = 0; i < round; i++)
                {
                    GameObject bullet3 = Instantiate(pBullet);
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
