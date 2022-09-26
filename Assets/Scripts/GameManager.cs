using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    // *목표 1 : 적을 생성하고 싶다.
    // *풀이 :
    //      1) 적 프리팹과 스폰 포인트를 불러온다.
    //      2) 배열 형태로 불러온다.
    //      3) 적이 생성되는 생성 시간을 지정한다.
    //      4) 현재 시간을 선언 한다.
    //      5) 생성 시간은 매번 랜덤하게 지정된다.
    //      6) 랜덤한 적을 생성한다.
    //      7) 랜덤한 위치에서 생성한다.

    // *목표 2 : 적이 생성될 때 그자리에서 방향을 지정하고 물리적인 움직임을 행한다.
    // *풀이 :
    //      1) 옆에 생성 되었을 때 새로운 속도( 속력 + 방향 )를 지정해 준다.
    //      2) Enemy에 있는 speed 변수를 사용한다.
    //      3) 방향을 바꿔준다.
    // *순서 :
    //      1) 스폰 포인트를 좌,우로 추가한다.
    //      2) Enemy스크립트에서 할당된 속도를 지운다.
    //      3) EnemyManager에서 자체적으로 속도를 할당한다.
    //      4) 이를 위해 Enemy스크립트에 있는 speed변수를 사용한다.
    //      5) 인스턴스 객체를 오브젝트에 저장하고
    //      6) 스폰 포인트에 따라 속도를 설정하여 준다.
    //      7) Rotate()함수를 통해 방향을 바꾼다.(z축은 forward(+1)와 back(-1)으로 조절할 수 있다.)

    public int stage;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;

    public string[] enemyFactory;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;

    public Image[] lifeImage;
    public Image[] boomImage;

    public ObjectManager objectManager;

    public GameObject gameOverSet;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyFactory = new string[]{"EnemyS", "EnemyM", "EnemyL", "EnemyB"};
        StageStart();
    }

    public void StageStart()
    {
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart!";
        clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear!";

        ReadSpawnFile();

        fadeAnim.SetTrigger("In");
    }
    public void StageEnd()
    {
        clearAnim.SetTrigger("On");

        stage++;

        fadeAnim.SetTrigger("Out");
    }

    void ReadSpawnFile()
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        TextAsset textFile = Resources.Load("Stage " + stage.ToString()) as TextAsset;
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
        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch(spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyFactory[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;
        
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;

        if(enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if(enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        spawnIndex++;
        if(spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life)
    {
        for(int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for(int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for(int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

                for(int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
