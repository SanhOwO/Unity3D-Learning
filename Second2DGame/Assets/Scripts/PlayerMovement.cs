using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;

    [Header("移动参数")]
    public float speed = 8;
    public float crouchSpeedDivisor = 3f;

    [Header("跳跃参数")]
    public float jumpFroce = 1f;
    public float superJump = 3f; 
    public float superJumpDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float grabJump = 15f;

    float jumpTime;

    public float xVelocity;//只能这里用 不写private

    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHead;
    public bool isHanging;

    [Header("环境检测")]
    public LayerMask groundLayer;

    public float foot = 0.4f;
    //public float head = 0.9f;   //col.size.y
    public float groundDistance = 0.2f;
    float playerHeight;     
    public float eyeHeight = 1.5f;
    public float  wallDistance = 0.4f;  //距离墙壁的距离
    public float reachOffset = 0.7f;    //射线的 头顶有空，头下无


    //colider size
    Vector2 colliderStandSize;
    Vector2 colliderStandoffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    //按建设者
    bool jumpPress;
    bool jumpHold;
    bool crouchPress;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        colliderStandSize = col.size;
        colliderStandoffset = col.offset;

        colliderCrouchSize = new Vector2(col.size.x, col.size.y / 2f);
        colliderCrouchOffset = new Vector2(col.offset.x, col.offset.y / 2f);

        playerHeight = col.size.y;
    }

    void Update()
    {
        if (GameManager.GameOver())
        {
            return;
        }
        //使用getbutton的时候 放进update让其更平滑,Down检测一下，不加一只检测
        jumpPress = Input.GetButtonDown("Jump");
        jumpHold = Input.GetButton("Jump");
        crouchPress = Input.GetButton("Crouch");
    }

    private void FixedUpdate()
    {
        if (GameManager.GameOver())
        {
            return;
        }
        //物理相关放进fixUpdate
        PhysicCheck();

        GroundMovement();

        AirMovement();
    }

    void AirMovement()
    {
        if (isHanging)
        {
            if (jumpPress)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, grabJump);
                isHanging = false;
            }else if (crouchPress)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false; 
            }
        }

        if(isOnGround && jumpPress && !isJump && !isHead)
        {
            if(isCrouch)
            {
                Standup();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true; 
            //rb.velocity = new Vector2(rb.velocity.x, jumpFroce);
            rb.AddForce(new Vector2(0f, jumpFroce),ForceMode2D.Impulse);

            jumpTime = Time.time + superJumpDuration;

            AudioManager.Jump();
            AudioManager.JumpPlayerVoice();
        }
        else if(isJump)
        {
            if (jumpHold)
                rb.AddForce(new Vector2(0f, superJump), ForceMode2D.Impulse);
            if (jumpTime < Time.time)
                isJump = false;
        }
    }

    void PhysicCheck()
    {
        /*Vector2 pos = transform.position;
        Vector2 offSet = new Vector2(-foot, 0f);
        RaycastHit2D leftcheck = Physics2D.Raycast(pos + offSet, Vector2.down, groundDistance, groundLayer);//投射射线从中间往右offset个身位往下，碰到groundlayer返回
        Debug.DrawRay(pos+offSet,Vector2.up,Color.red,0.2f);*/
        RaycastHit2D leftcheck = Raycast(new Vector2(-foot,0f), Vector2.down, groundDistance, groundLayer);//投射射线从中间往右offset个身位往下，碰到groundlayer返回
        RaycastHit2D Rightcheck = Raycast(new Vector2(foot,0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D headcheck = Raycast(new Vector2(0f, col.size.y), Vector2.up, groundDistance, groundLayer);
        if (leftcheck || Rightcheck)
            isOnGround = true;
        else
            isOnGround = false;

        if (headcheck)
            isHead = true;
        else
            isHead = false;
        //挂壁
        float direction = transform.localScale.x;
        Vector2 graphDir = new Vector2(direction, 0f);
        RaycastHit2D grabCheck = Raycast(new Vector2(foot * direction, playerHeight), graphDir, wallDistance, groundLayer); //脑袋放射线 前方 要求无碰撞
        RaycastHit2D wallCheck = Raycast(new Vector2(foot * direction, eyeHeight), graphDir, wallDistance, groundLayer); //眼睛放射线 前方 要求有碰撞
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, wallDistance, groundLayer); //前方向下 要求有碰撞

        if (!isOnGround && rb.velocity.y < 0 && ledgeCheck && wallCheck && !grabCheck)
        {
            //调整位置
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance-0.05f) * direction;  //射线与碰撞体之间的距离 distance
            pos.y -= ledgeCheck.distance;
            transform.position = pos;

            //固定
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }

    }  

     void GroundMovement()
    {
        if (isHanging) return;

        if (crouchPress && !isCrouch && isOnGround)
            Crouch();
        else if (!crouchPress && isCrouch && !isHead)
            Standup();
        else if (!isOnGround && isCrouch && !isHead)
            Standup();

        xVelocity = Input.GetAxis("Horizontal"); //-1f 1f

        if (isCrouch)
            xVelocity /= crouchSpeedDivisor;

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        FlipDirection();
    }

    void FlipDirection()
    {
        if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Crouch()
    {
        isCrouch = true;
        col.size = colliderCrouchSize;
        col.offset = colliderCrouchOffset;
    }

    void Standup()
    {
        isCrouch = false;
        col.size = colliderStandSize;
        col.offset = colliderStandoffset;
    }

    //写一个新的raycast函数 方便调用
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection * length, color);

        return hit;
    }
}
