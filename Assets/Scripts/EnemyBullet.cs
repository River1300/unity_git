using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BoarderW")
        {
            gameObject.SetActive(false);
        }
    }
}
