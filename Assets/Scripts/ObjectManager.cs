using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;

    public GameObject pBulletPrefab;
    public GameObject eBulletPrefab;
    public GameObject fBulletPrefab;

    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;

    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] pBullet;
    GameObject[] eBullet;
    GameObject[] fBullet;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] target;

    void Awake()
    {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        pBullet = new GameObject[300];
        eBullet = new GameObject[200];
        fBullet = new GameObject[100];

        itemCoin = new GameObject[10];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        Generate();
    }

    void Generate()
    {   // #. enemy
        for(int i = 0; i < enemyL.Length; i++)
        {
            enemyL[i] = Instantiate(enemyLPrefab);
            enemyL[i].SetActive(false);
        }
        for(int i = 0; i < enemyM.Length; i++)
        {
            enemyM[i] = Instantiate(enemyMPrefab);
            enemyM[i].SetActive(false);
        }
        for(int i = 0; i < enemyS.Length; i++)
        {
            enemyS[i] = Instantiate(enemySPrefab);
            enemyS[i].SetActive(false);
        }

        // #. bullet
        for(int i = 0; i < pBullet.Length; i++)
        {
            pBullet[i] = Instantiate(pBulletPrefab);
            pBullet[i].SetActive(false);
        }
        for(int i = 0; i < eBullet.Length; i++)
        {
            eBullet[i] = Instantiate(eBulletPrefab);
            eBullet[i].SetActive(false);
        }
        for(int i = 0; i < fBullet.Length; i++)
        {
            fBullet[i] = Instantiate(fBulletPrefab);
            fBullet[i].SetActive(false);
        }

        // #. item
        for(int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoinPrefab);
            itemCoin[i].SetActive(false);
        }
        for(int i = 0; i < itemPower.Length; i++)
        {
            itemPower[i] = Instantiate(itemPowerPrefab);
            itemPower[i].SetActive(false);
        }
        for(int i = 0; i < itemBoom.Length; i++)
        {
            itemBoom[i] = Instantiate(itemBoomPrefab);
            itemBoom[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch(type)
        {
            case "EnemyL":
                target = enemyL;
                break;

            case "EnemyM":
                target = enemyM;
                break;

            case "EnemyS":
                target = enemyS;
                break;

            case "pBullet":
                target = pBullet;
                break;

            case "eBullet":
                target = eBullet;
                break;

            case "fBullet":
                target = fBullet;
                break;

            case "ItemCoin":
                target = itemCoin;
                break;

            case "ItemPower":
                target = itemPower;
                break;

            case "ItemBoom":
                target = itemBoom;
                break;
        }
        for(int i = 0; i < target.Length; i++)
        {
            if(!target[i].activeSelf)
            {
                target[i].SetActive(true);
                return target[i];
            }
        }
        return null;
    }

    public GameObject[] GetPool(string name)
    {
        switch(name)
        {
            case "EnemyL":
                target = enemyL;
                break;

            case "EnemyM":
                target = enemyM;
                break;

            case "EnemyS":
                target = enemyS;
                break;

            case "pBullet":
                target = pBullet;
                break;

            case "eBullet":
                target = eBullet;
                break;

            case "fBullet":
                target = fBullet;
                break;

            case "ItemCoin":
                target = itemCoin;
                break;

            case "ItemPower":
                target = itemPower;
                break;

            case "ItemBoom":
                target = itemBoom;
                break;
        }
        return target;
    }

}
