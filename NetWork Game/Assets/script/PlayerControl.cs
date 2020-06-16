using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //重启游戏
using Photon.Pun;

public class PlayerControl : MonoBehaviourPun
{
    //角色---------------
    public Rigidbody2D rb;
    private Collider2D ccoll;
    public Collider2D disCol;
    public Animator aim;
    public Text nameText;

    SpriteRenderer sprite;
    [Space]
    public float speed = 10.0f,jumpForce = 5;
    public Transform groundCheck;
    public Transform cellingCheck;
    public LayerMask ground;
    [Space]
    public bool isGround, isJump;

    bool jumpPress;
    int jumpCount;

    [Space]
    private int cherry;
    public Text cherryCount;

    private bool ishurt;
    //[Space]
    //public AudioSource jumpAudio;
    //public AudioSource collectAudio;

    //物品------------------
    //public Collider2D[] scoll;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ccoll = GetComponent<CircleCollider2D>();
        aim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //scoll = GetComponent<Collider2D[]>();
        //cherryCount = GetComponent<Text>();
        //groundcheck = GetComponent<Transform>();
        cherry = 0;
        ishurt = false;

        if (photonView.IsMine)
            nameText.text = PhotonNetwork.NickName;
        else
            nameText.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPress = true;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        //groundcheck = GameObject.Find("Groundcheck").transform;
        if (!ishurt)
        {
            isGround = Physics2D.OverlapCircle(groundCheck.position,0.1f,ground);

            GroundMovement();

            Jump();
        }

        SwitchAnim(); 
    }

    private void GroundMovement()
    {
        float hor = Input.GetAxisRaw("Horizontal");//-1 到 1  raw代表-1,1
        rb.velocity = new Vector2(hor * speed, rb.velocity.y);
        if (hor != 0)
        {
            if (hor > 0)
                sprite.flipX = false;
            else
                sprite.flipX = true;
            //transform.localScale = new Vector3(hor, 1, 1);
        }
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if(jumpPress && isGround)
        {
            //SoundManager sound = gameObject.GetComponent<SoundManager>();
            //sound.JumpAudio();
            //jumpAudio.Play();
            SoundManager.instance.JumpAudio();
            isJump = true;
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.velocity = Vector2.up * jumpForce;
            jumpCount--;
            jumpPress = false;
        } 
        else if(jumpPress && jumpCount > 0 && !isGround)
        {
            SoundManager.instance.JumpAudio();
            //jumpAudio.Play();
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.velocity = Vector2.up * jumpForce;
            jumpCount--;
            jumpPress = false;
        }
    }

    void SwitchAnim()
    {
        aim.SetFloat("running", 0);

        if (isGround)
        {
            aim.SetFloat("running", Mathf.Abs(rb.velocity.x));
            aim.SetBool("falling", false);
            aim.SetBool("jumping", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            aim.SetBool("jumping", true);
            aim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y < 0)
        {
            aim.SetBool("falling", true);
            aim.SetBool("jumping", false);
        }

        if (ishurt)
        {
            aim.SetBool("hurt", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1)//如果受伤停下来了
            {
                Debug.Log("stop");
                ishurt = false;
                aim.SetBool("hurt", false);
            }
        }

        Crouch();
    }
    //收集
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "collection")
        {
            Destroy(collision.gameObject);
            //UI
            //collectAudio.Play();
            SoundManager.instance.cherryAudio();
            cherry++;
            cherryCount.text = cherry.ToString();
        }
        if(collision.tag == "DeadLine")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);//当前scene
            //SceneManager.LoadScene("1");
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);//延时呼叫函数
        }
    }

    //Enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Frog frog = collision.gameObject.GetComponent<Frog>();
        Enemy allEnemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.gameObject.tag == "Enemy")
        {
            if (aim.GetBool("falling"))
            {
                allEnemy.JumpOn(); 
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                aim.SetBool("jumping", true);
            }
            else if(transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10 * Time.deltaTime, rb.velocity.y);
                ishurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10 * Time.deltaTime, rb.velocity.y);
                ishurt = true;
            }
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            aim.SetBool("crouching", true);
            disCol.enabled = false;
        }
        else
        {
            if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
            {
                 aim.SetBool("crouching", false);
                 disCol.enabled = true;
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//当前scene
    }
}
