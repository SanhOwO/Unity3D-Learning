using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    public Rigidbody2D rb;
    public Transform left, right;
    [Space]
    public float speed = 2;
    public float jump = 10;
    private float leftx;
    private float rightx;
    private bool faceleft = true;
    public Collider2D col;
    public LayerMask ground;
    [Space]
    //private Animator anim;
    public bool isGround, isJump=false;

    // Start is called before the first frame update
    protected override void Start() //protected为了调用父级Enemy的函数,override为了修改父级函数
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();

        leftx = left.position.x;
        rightx = right.position.x;

        Destroy(left.gameObject);
        Destroy(right.gameObject);

        anim = GetComponent<Animator>();
        //col = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();   //在idle的animation里加了event自动调用
        isGround = col.IsTouchingLayers(ground);
        SwitchAnimation();
    }

    void Movement()
    {
        if (faceleft)
        {
            if (isGround)//在地上就跳
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jump);
                isJump = true;
            }
            if (transform.position.x < leftx)//碰到界限转向
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceleft = false;
            }
        }
        else
        {
            if (isGround)
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jump);
                isJump = true;
            }
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceleft = true;
            }
        }
    }

    void SwitchAnimation()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (isGround && anim.GetBool("falling"))
        { 
            isJump = false;
        }
    }

    
}
