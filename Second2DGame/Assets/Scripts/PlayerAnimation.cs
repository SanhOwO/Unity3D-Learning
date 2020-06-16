using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement movement;
    private Rigidbody2D rb;

    int speedId;
    int groundId;
    int crouchId;
    //int jumpId;
    int hangId;
    int fallId;


    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        speedId = Animator.StringToHash("speed");
        groundId = Animator.StringToHash("isOnGround");
        crouchId = Animator.StringToHash("isCrouching");
        //jumpId = Animator.StringToHash("isJumping");
        hangId = Animator.StringToHash("isHanging");
        fallId = Animator.StringToHash("verticalVelocity");
    }


    void Update()
    {
        //anim.SetFloat("speed", Mathf.Abs(movement.xVelocity));
        //anim.SetBool("isOnGround", movement.isOnGround);
        //anim.SetBool("isCrouching", movement.isCrouch);
        anim.SetFloat(speedId, Mathf.Abs(movement.xVelocity));
        anim.SetBool(groundId, movement.isOnGround);
        anim.SetBool(crouchId, movement.isCrouch);
        anim.SetBool(hangId, movement.isHanging);
        //跳跃动画由多个动画组成，在Animator里用blend tree实现， 从-12 到10 代表下落到跳跃，根绝rb的y轴向量决定
        anim.SetFloat(fallId, rb.velocity.y);
    }

    public void SetpAudio()
    {
        AudioManager.WalkAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.CrouchWalkAudio();
    }

}
