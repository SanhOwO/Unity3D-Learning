using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;

    public float speed, jumpForce;
    private float horizontalMove;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump, isDashing;

    bool jumpPressed;
    int jumpCount;

    [Header("Dash")]
    public float dashTime;//冲锋时间
    private float dashTimeLeft;//冲锋剩余时间
    public float cd;
    private float lastDash;//上次dash时间点
    public float dashSpeed;

    [Header("CDImage")]
    public Image CDImage;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        lastDash = cd + 1;//防止一开始无法使用
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(Time.time >= (lastDash + cd))
            {
                //可以执行dash
                Ready2Dash();
            }
        }
        //API手册上有
        CDImage.fillAmount -= 1.0f / cd * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);

        Dash();
        if (isDashing)
            return;
        GroundMovement();

        Jump();


        SwitchAnim();
    }

    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");//只返回-1，0，1
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }

    }

    void Jump()//跳跃
    {
        if (isGround)
        {
            jumpCount = 2;//可跳跃数量
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void SwitchAnim()//动画切换
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }

    void Ready2Dash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        CDImage.fillAmount = 1;
    }

    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if (rb.velocity.y > 0 && !isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);
                }
                rb.velocity = new Vector2(dashSpeed * horizontalMove, rb.velocity.y);

                //因为要在fixupdate调用，所以用deltaTime
                //time是从程序开始执行到现在的时间，deltaTime上一帧完成的时间，fixedTime表示FixedUpdate已经执行的时间，而fixedDeltatime是一个固定的时间增量。
                dashTimeLeft -= Time.deltaTime;

                ObjectPool.instance.SpawnPool();
            }
            else
            {
                isDashing = false;
                if (!isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);
                }
            }
        }


    }
}
