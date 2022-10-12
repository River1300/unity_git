using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public new string name;
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rigid.velocity = Vector3.down * 0.5f;
    }
}
