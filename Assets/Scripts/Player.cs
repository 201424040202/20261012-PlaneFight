using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int frameRate = 10;
    public Sprite[] idleSprites;

    private SpriteRenderer spriteRenderer;
    private float timer = 0;
    private int currentFrame = 0;

    public Vector3 lastMousePosition = Vector3.zero;

    public bool isMouseDown = false;

    public float superGunDuration = 3;
    private float superGunTimer = -1;

    public GameObject gunTop;
    public GameObject gunRight;
    public GameObject gunLeft;

    private float invincibleTime = 2;
    private bool isInvincible = false;
    private float invincibleTimer = 0;

    public float blinkInterval = 0.2f;//闪烁的时间间隔

    //帧动画中的帧
    public Sprite[] deathSprites;

    public AudioSource getBombAudio;
    public AudioSource getSuperGunAudio;

    // 玩家碰撞体
    private Collider2D playerCollider;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 玩家碰撞体
        playerCollider = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        IdleAnimationUpdate();
        MoveUpdate();
        SuperGunUpdate();
        DeathAnimationUpdate();
    }

    void IdleAnimationUpdate()
    {
        if (isDead) return;
        timer += Time.deltaTime;
        if(timer > 1f/frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % idleSprites.Length;
            spriteRenderer.sprite = idleSprites[currentFrame];
        }
    }

    void DeathAnimationUpdate()
    {
        if (!isDead) return;
        
        if(currentFrame >= deathSprites.Length)
        {
            //GameOver
            GameManager.Instance.GameOver();
        }
        else
        {
            spriteRenderer.sprite = deathSprites[currentFrame];
        }

        timer += Time.deltaTime;
        if (timer > 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame++;
        }
        
    }

    void MoveUpdate()
    {
        if (GameManager.Instance.IsPause())
        {
            return;
        }
        if (!GameManager.Instance.IsAlive()) return;

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            // 按下瞬间：鼠标屏幕坐标 → 世界坐标
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // z值固定，避免相机深度影响
            lastMousePosition.z = transform.position.z;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
        if (isMouseDown)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z;

            Vector3 offset = mouseWorldPos - lastMousePosition;
            transform.position += offset;

            CheckPosition();
            lastMousePosition = mouseWorldPos;
        }
    }

    void CheckPosition()
    {
        //x 2.13
        //y -3.78 3.45

        Vector3 pos = transform.position;

        if (pos.x <-2.13f)
        {
            pos.x = -2.13f;
        }
        if (pos.x > 2.13f)
        {
            pos.x = 2.13f;
        }
        if (pos.y < -3.78f)
        {
            pos.y = -3.78f;
        }
        if (pos.y > 3.45f)
        {
            pos.y = 3.45f;
        }

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isDead)
        {
            return;
        }

        if(collision.tag == "Award")
        {
            if(collision.GetComponent<Award>().awardType == AwardType.SuperGun)
            {
                TransformToSuperGun();
                getSuperGunAudio.Play();
            }
            else if(collision.GetComponent<Award>().awardType == AwardType.Life)
            {
                GameManager.Instance.AddLife();
            }
            else
            {
                getBombAudio.Play();
                GameManager.Instance.AddBomb();
            }
            Destroy(collision.gameObject);
        }

        // 无敌时无法和敌人发生碰撞
        if(collision.tag == "Enemy" && isInvincible == false)
        {
            collision.SendMessage("TakeDamage");
            GameManager.Instance.SubLife();
            if (!GameManager.Instance.IsAlive())
            {
                // 死亡爆炸
                TransformToDeath();
            }
            else
            {
                // 闪烁
                TransformToInvincible();
            }
        }
    }

    void SuperGunUpdate()
    {
        
        if (isDead)
        {
            return;
        }

        if(superGunTimer >= 0)
        {
            superGunTimer -= Time.deltaTime;

            if(superGunTimer <= 0)
            {
                TransformToNormalGun();
            }
            
        }
    }

    // 进入到了无敌状态
    void TransformToInvincible()
    {
        isInvincible = true;
        invincibleTimer = 0;

        //闪烁动画
        StartCoroutine(BlinkEffect());
    }

    void TransformToSuperGun()
    {
        gunRight.SetActive(true);
        gunLeft.SetActive(true);
        gunTop.SetActive(false);

        superGunTimer = superGunDuration;
    }

    void TransformToNormalGun()
    {
        gunRight.SetActive(false);
        gunLeft.SetActive(false);
        gunTop.SetActive(true);
    }

    void DisableAllGun()
    {
        gunRight.SetActive(false);
        gunLeft.SetActive(false);
        gunTop.SetActive(false);
    }

    // 进入死亡处理的动画
    void TransformToDeath()
    {

        // 防止重复进入
        if (isDead){
            return;
        }

        isDead = true;

        DisableAllGun();

        if(playerCollider != null)
        {
            // 不再接收敌人和奖励的触发
            playerCollider.enabled = false;
        }



        timer = 0; // 计时器归零
        currentFrame = 0;
        //this.enabled = false;
    }

    //协程实现闪烁
    IEnumerator BlinkEffect()
    {
        while(invincibleTimer <= invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            invincibleTimer += blinkInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
