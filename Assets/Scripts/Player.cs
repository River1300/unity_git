using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // *목표 1 : Player가 사용자 입력에 따라 이동하게 하고 싶다.
    // *풀이 : 
    //      1) Player 게임 오브젝트가 움직인다.
    //      2) 사용자가 입력한 방향에 따라 움직인다.
    //      3) 지정한 속도에 따라 움직인다.
    // *필요속성 : 방향, 속도
    //          => 방향 : GetAxisRaw(-1, 0, +1)를 통해 방향을 입력 받는다.
    // *구현 : 움직이는 것은 게임 중 계속해서 일어난다. Update()
    //          => P = P0 + vt;
    // *순서 :
    //      1) 속성으로 사용할 속도 변수를 만든다.
    //      2) Update()함수에 지속적으로 바뀌는 방향 속성을 만든다.
    //          => 방향은 사용자 입력에 따라 수직, 수평 값을 받는다.
    //      3) 현재 위치와
    //      4) 미래 위치를
    //      5) 더하여 Player을 움직인다.

    // *목표 2 : Player가 화면 밖으로 나가지 못하게 하고 싶다.
    // *풀이 : 
    //      1) Player가 경계선에 닿으면
    //      2) 해당 방향값이 0으로 바뀐다.
    // *필요속성 : BoxCollider2D, RigidBody2D, 상하좌우 경계선, bool 변수
    // *구현 : 만약 Player가 경계선에 닿으면, 그 방향 값은 0이 된다.
    //      => if문과 swich문을 적절히 사용한다.
    // *순서 :
    //      1) Player가 경계선 상에 닿았는지 확인해 줄 bool 타입의 변수를 만든다.
    //      2) 닿았을 때 호출되는 함수, 물리적 충돌이 아닌 isTrigger 함수를 만든다.
    //      3) Player가 해당 경계선에 닿으면, 그 경계선을 뜻하는 bool 변수를 true로 바꿔준다.
    //      4) true로 바뀐 변수를 사용하여 해당 방향 값을 0으로 바꾼다.
    //      5) 다시 경계선을 벗어날 경우 Exit2D bool 변수를 false로 바꿔준다.

    // *목표 3 : Player의 움직임 방향에 따라 각기 다른 애니메이션을 출력하고 싶다.
    // *풀이 : 
    //      1) 왼쪽, 오른쪽 방향으로 움직일 때
    //      2) -1, 1의 파라미터를 애니메이터에 전달한다.
    // *필요속성 : 애니메이션, 애니메이터, 스프라이트, 파라미터, 애니메이션 변수
    // *구현 : 키를 눌렀을 때와 때었을 때 애니메이터에서 만들어 놓은 파라미터에 값을 전달한다.
    // *순서 :
    //      1) 애니메이션을 작동시키기 위해 Animator 클래스의 객체를 만들어 준다.
    //      2) Awake() 함수로 Animator 객체를 초기화 시켜 놓는다.
    //      3) 애니메이션은 좌우로만 움직이므로 Horizontal 방향을 입력 받았을 때 바뀐다.
    //      4) 입력된 버튼의 값(-1, 1)을 애니메이터에서 등록해 둔 파라미터에 전달해 준다.
    //          => 애니메이터 클래스의 멤버 변수 SetInter()로 파라미터에 값을 전달한다.
    //          => 방향 값은 float이고 파라미터 값은 int이므로 강제 형변환이 필요하다.

    // *목표 4 : 캡슐화 시키고 싶다.

    // *목표 5 : 플레이어가 총알을 발사하고 싶다.
    // *풀이 :
    //      1) 프리팹에서 공장을 받아온다.
    //      2) 총알 객체를 만든다.
    // *필요속성 : 총알 공장
    // *순서 :
    //      1) 공장을 GameObject로 받아온다.
    //      2) 공장을 사용해 객체를 만든다.
    //      3) Rigidbody컴포넌트의 AddForce()함수를 이용한다.
    //      4) 만들어진 객체를 통해 Rigidbody컴포넌트를 받아온다.

    // *목표6 : 발사 버튼을 눌렀을 때만 발사하고 싶다.
    // *풀이 :
    //      1) 사용자가 발사 버튼을 누르고 있지 않다면
    //      2) 총알이 발사되지 않는다.
    // *필요속성 : 딜레이 시간, 현재 시간
    // *순서 : 
    //      1) 현재 시간이 계속해서 추가된다.
    //      2) 만약 현재 시간이 발사 시간보다 작다면
    //      3) 발사되지 않는다.

    // *목표 7 : 파워에 수치에 따라 총알이 다르게 발사된다.
    // *풀이 :
    //      1) 파워 변수의 값이 바뀌면
    //      2) 1단계 2단계 3단계로 나뉘 switch문으로 간다.
    //      3) 2단계는 작은 총알을 2개 발사한다.
    //      4) 총알의 위치를 조정해 준다.
    //      5) 각각 발사된다.

    public float speed;
    public int power;
    public int maxPower;
    public int boom;
    public int maxBoom;
    public float maxShotDelay;
    public float currentDelay;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public int life;
    public int score;

    public GameObject bulletFactoryA;
    public GameObject bulletFactoryB;
    public GameObject boomEffect;

    public GameManager gameManager;
    public ObjectManager objectManager;

    public bool isHit;
    public bool isBoomTime;

    public GameObject[] followers;
    public bool isRespawnTime;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;

    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 3);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if(isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            for(int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color 
                    = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);

            for(int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color 
                    = new Color(1, 1, 1, 1);
            }
        }
    }

    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }

    public void ButtonAUp()
    {
        isButtonA = false;
    }

    public void ButtonBDown()
    {
        isButtonB = true;
    }

    void Fire()
    {
        //if(!Input.GetButton("Fire1")) { return; }

        if(!isButtonA) { return; }
        
        if(currentDelay < maxShotDelay){ return; }


        switch(power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 2:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.2f;
                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.2f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 3:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.3f;
                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.3f;
                GameObject bulletCC = objectManager.MakeObj("BulletPlayerB");
                bulletCC.transform.position = transform.position;
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 4:
            case 5:
            case 6:
            case 7:
                GameObject bulletRRR = objectManager.MakeObj("BulletPlayerB");
                bulletRRR.transform.position = transform.position + Vector3.right * 0.3f;
                GameObject bulletLLL = objectManager.MakeObj("BulletPlayerB");
                bulletLLL.transform.position = transform.position + Vector3.left * 0.3f;
                GameObject bulletCCC = objectManager.MakeObj("BulletPlayerB");
                bulletCCC.transform.position =transform.position;

                bulletRRR.transform.Rotate(Vector3.back * 45);
                bulletLLL.transform.Rotate(Vector3.forward * 45);

                Rigidbody2D rigidRRR = bulletRRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLLL = bulletLLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCCC = bulletCCC.GetComponent<Rigidbody2D>();
                rigidRRR.AddForce(new Vector2(1, 2) * 3, ForceMode2D.Impulse);
                rigidLLL.AddForce(new Vector2(-1, 2) * 3, ForceMode2D.Impulse);
                rigidCCC.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                break;
        }
        currentDelay = 0;
    }

    void Reload()
    {
        currentDelay += Time.deltaTime;
    }

    void Boom()
    {
        //if(!Input.GetButton("Fire2")) { return; }

        if(!isButtonB) { return; }

        if(isBoomTime) { return; }

        if(boom == 0) { return; }

        boom--;

        isBoomTime = true;

        gameManager.UpdateBoomIcon(boom);

        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 2f);

        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for(int index = 0; index < enemiesL.Length; index++)
        {
            if(enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for(int index = 0; index < enemiesM.Length; index++)
        {
            if(enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for(int index = 0; index < enemiesS.Length; index++)
        {
            if(enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");

        for(int index = 0; index < bulletsA.Length; index++)
        {
            if(bulletsA[index].activeSelf)
            {
                bulletsA[index].SetActive(false);
            }
        }
        for(int index = 0; index < bulletsB.Length; index++)
        {
            if(bulletsB[index].activeSelf)
            {
                bulletsB[index].SetActive(false);
            }
        }
    }

    public void JoyPanel(int type)
    {
        for(int index = 0; index < 9; index++)
        {
            joyControl[index] = index == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }

    public void JoyUp()
    {
        isControl = false;
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(joyControl[0]) { h = -1; v = 1; }
        if(joyControl[1]) { h = 0; v = 1; }
        if(joyControl[2]) { h = 1; v = 1; }
        if(joyControl[3]) { h = -1; v = 0; }
        if(joyControl[4]) { h = 0; v = 0; }
        if(joyControl[5]) { h = 1; v = 0; }
        if(joyControl[6]) { h = -1; v = -1; }
        if(joyControl[7]) { h = 0; v = -1; }
        if(joyControl[8]) { h = -1; v = -1; }

        if((isTouchRight && h == 1) || (isTouchLeft && h == -1) || !isControl){ h = 0; }
        if((isTouchTop && v == 1) || (isTouchBottom && v == -1) || !isControl){ v = 0; }

        Vector3 currentPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = currentPos + nextPos;

        if(Input.GetButtonDown("Horizontal") ||
        Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Border")
        {
            switch(other.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Enemy Bullet")
        {
            if(isRespawnTime) { return; }
            if(isHit){ return; }
            isHit = true;
            life--;
            gameManager.UpdateLifeIcon(life);
            gameManager.CallExplosion(transform.position, "P");

            if(life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "Item")
        {
            Item item = other.gameObject.GetComponent<Item>();
            switch(item.type)
            {
                case "Coin" :
                    score += 1000;
                    break;
                    
                case "Power" :
                    if(power == maxPower) { score += 500; }
                    else 
                    { 
                        power++;
                        AddFollower();
                    }
                    break;

                case "Boom" :
                    if(boom == maxBoom) { score += 500; }
                    else 
                    { 
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            other.gameObject.SetActive(false);
        }
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
                if(other.gameObject.tag == "Border")
        {
            switch(other.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }

    void AddFollower()
    {
        if(power == 5)
        {
            followers[0].SetActive(true);
        }
        else if(power == 6)
        {
            followers[1].SetActive(true);
        }
        else if(power == 7)
        {
            followers[2].SetActive(true);
        }
    }
}
