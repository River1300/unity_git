using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyFactors;
    public Transform[] spawnPoints;
    public GameObject player;

    public Text scoreText;
    public Image[] playerLife;
    public Image[] boomCount;
    public GameObject gameOverSet;

    public float maxSpawnTime;
    float curSpawnTime;

    int ranType;
    int ranPoint;

    Rigidbody rigid;

    void Update()
    {
        FlowTime();
        if(curSpawnTime > maxSpawnTime)
        {
            SpawnEnemy();
            maxSpawnTime = Random.Range(0.5f, 3f);
            curSpawnTime = 0;
        }
        CoutScore();
    }

    void FlowTime()
    {
        curSpawnTime += Time.deltaTime;
    }

    void SpawnEnemy()
    {
        ranType = Random.Range(0, 3);
        ranPoint = Random.Range(0, 9);
        GameObject enemy = Instantiate(enemyFactors[ranType],
                            spawnPoints[ranPoint].position,
                            spawnPoints[ranPoint].rotation);
        rigid = enemy.GetComponent<Rigidbody>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if(ranPoint == 5 || ranPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * (-90));
            rigid.velocity = new Vector3(enemyLogic.speed * (-1), -1, 0);
        }
        else if(ranPoint == 7 || ranPoint == 8)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector3(enemyLogic.speed, -1, 0);
        }
        else
        {
            rigid.velocity = Vector3.down * enemyLogic.speed;
        }
    }

    public void CoutLife(int life)
    {
        for(int i = 0; i < 3; i++)
        {
            playerLife[i].color = new Color(1, 1, 1, 0);
        }
        for(int i = 0; i < life; i++)
        {
            playerLife[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void CountBoom(int boom)
    {
        for(int i = 0; i < 3; i++)
        {
            boomCount[i].color = new Color(1, 1, 1, 0);
        }
        for(int i = 0; i < boom; i++)
        {
            boomCount[i].color = new Color(1, 1, 1, 1);
        }
    }

    void CoutScore()
    {
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    public void RespawnCall()
    {
        Invoke("RespawnPlayer", 2f);
    }
    void RespawnPlayer()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOverSet()
    {
        gameOverSet.SetActive(true);
    }

    public void GameScene()
    {
        SceneManager.LoadScene(0);
    }
}
