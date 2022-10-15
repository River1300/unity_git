using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public bool isRotate;
    
    void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void Update()
    {
        if(isRotate){
            transform.Rotate(Vector3.forward * 10);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BoarderW")
        {
            gameObject.SetActive(false);
        }
    }
}