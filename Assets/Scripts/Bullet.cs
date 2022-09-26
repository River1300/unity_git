using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // *목표 : 총알이 새로운 경계선과 부딪치면 총알을 제거하고 싶다.
    // *풀이 :
    //      1) Trigger
    //      2) 총알이 다른 Collider와 부딛쳤다.
    //      3) 총알이 제거 된다.
    // *필요속성 : BoxCollider, Rigidbody

    public int dmg;
    public bool isRotate;

    void Update()
    {
        if(isRotate) { transform.Rotate(Vector3.forward * 10); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }
}
