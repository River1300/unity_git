using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Material bgMaterial;
    public float speed;

    void Update()
    {
        Vector2 dir = Vector2.up;

        bgMaterial.mainTextureOffset += dir * speed * Time.deltaTime;
    }
}
