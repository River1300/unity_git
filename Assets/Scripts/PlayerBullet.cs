using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage;

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
            gameObject.transform.rotation = Quaternion.identity;
        }
    }
}
