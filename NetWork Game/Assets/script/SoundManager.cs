using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;//单链模式 创建个实体的类
    public AudioSource audioSource;
    [SerializeField]
    public AudioClip jump, hurt, cherry;

    private void Awake()
    {
        instance = this;    
    }

    public void JumpAudio()
    {
        audioSource.clip = jump;
        audioSource.Play();
    }

    public void cherryAudio()
    {
        audioSource.clip = cherry;
        audioSource.Play();
    }
}
