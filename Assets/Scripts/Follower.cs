using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float fireTime;
    float curTime = 0;
    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    Rigidbody rigid;

    void Awake()
    {
        parentPos = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()
    {
        if(!parentPos.Contains(parent.position + Vector3.back * 0.5f))
        {
            parentPos.Enqueue(parent.position + Vector3.back * 0.5f);
        }

        if(parentPos.Count > followDelay)
        {
            followPos = parentPos.Dequeue();
        }
        else if(parentPos.Count < followDelay)
        {
            followPos = parent.position + Vector3.back * 0.5f;
        }
    }

    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if(!Input.GetButton("Fire1")) { return; }

        if(curTime < fireTime) { return; }

        GameObject bullet = objectManager.MakeObj("fBullet");
        bullet.transform.position = transform.position;
        rigid = bullet.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);

        curTime = 0;
    }

    void Reload()
    {
        curTime += Time.deltaTime;
    }
}
