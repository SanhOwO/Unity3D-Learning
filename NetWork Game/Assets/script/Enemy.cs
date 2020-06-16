using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathAudio;


    protected virtual void Start()  //protected可被子集调用，virtual可被子集修改
    {
        anim = GetComponent<Animator>();
        //deathAudio.GetComponent<AudioSource>();
    }

    public void death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        Debug.Log("death");
        anim.SetBool("death", true);
        //deathAudio.Play();
        GetComponent<Collider2D>().enabled = false;
    }


}
