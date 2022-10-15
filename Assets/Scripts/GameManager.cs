using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public string[] enemyFactors;
    public Transform[] spawnPoints;
    public GameObject player;

    public Text scoreText;
    public Text bestScoreText;
    public Image[] playerLife;
    public Image[] boomCount;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public float nextSpawnTime;
    float curSpawnTime;

    int ranType;
    int ranPoint;

    Rigidbody rigid;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyFactors = new string[]{ "EnemyL", "EnemyM", "EnemyS", "EnemyB" };
        ReadSpawnFile();
    }

    void Update()
    {
        FlowTime();
        if(curSpawnTime > nextSpawnTime && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnTime = 0;
        }
        CoutScore();
    }

    void ReadSpawnFile()
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        TextAsset textFile = Resources.Load("Stage 0") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);
            if(line == null) { break; }

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }
        stringReader.Close();

        nextSpawnTime = spawnList[0].delay;
    }

    void FlowTime()
    {
        curSpawnTime += Time.deltaTime;
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch(spawnList[spawnIndex].type)
        {
            case "B":
                enemyIndex = 3;
                break;
            case "L":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "S":
                enemyIndex = 2;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyFactors[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;
        rigid = enemy.GetComponent<Rigidbody>();

        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if(enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * (-90));
            rigid.velocity = new Vector3(enemyLogic.speed * (-1), -1, 0);
        }
        else if(enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector3(enemyLogic.speed, -1, 0);
        }
        else
        {
            rigid.velocity = Vector3.down * enemyLogic.speed;
        }

        spawnIndex++;
        if(spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        nextSpawnTime = spawnList[spawnIndex].delay;
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
        bestScoreText.text = string.Format("{0:n0}", playerLogic.bestScore);
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
