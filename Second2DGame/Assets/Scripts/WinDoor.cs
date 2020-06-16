using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    Animator anim;

    public Collider2D winZoon;

    int OpenID;

    private void Start()
    {
        anim = GetComponent<Animator>();

        OpenID = Animator.StringToHash("Open");

        GameManager.setDoor(this);

    }

    /*private void Update()
    {
        if (GameManager.collectAll())
            anim.SetTrigger(OpenID);
    }*/

    public void Open()
    {
        anim.SetTrigger(OpenID);
        AudioManager.OpenDoorVoice();
    }
}
