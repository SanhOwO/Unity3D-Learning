using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //allow other C# use, make it static
    static AudioManager current;

    [Header("BGM")]
    public AudioClip AmbientClip;
    public AudioClip musicClip;
    [Header("FX")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    [Header("Player")]
    public AudioClip[] wallk;
    public AudioClip[] crouchWalk;
    public AudioClip jump;
    public AudioClip jumpPlayerVoice;

    public AudioClip deathClip;
    public AudioClip dethVoiceClip;

    public AudioClip orbVoiceClip;

    public AudioClip doorOpenClip;

    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource fxSource;
    AudioSource playerSource;
    AudioSource voiceSource;

    private void Awake()
    {
        if(current != null)
        {
            Destroy(gameObject);
            return;
        } 
        current = this;
        //加载新图不要更新
        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();
    }

    public static void WalkAudio()
    {
        int index = Random.Range(0, current.wallk.Length);

        current.playerSource.clip = current.wallk[index];
        current.playerSource.Play();

    }

    public static void CrouchWalkAudio()
    {
        int index = Random.Range(0, current.crouchWalk.Length);

        current.playerSource.clip = current.crouchWalk[index];
        current.playerSource.Play();

    }

    public static void Jump()
    {
        current.playerSource.clip = current.jump;
        current.playerSource.Play();
    }

    public static void JumpPlayerVoice()
    {
        current.playerSource.clip = current.jumpPlayerVoice;
        current.playerSource.Play();
    }

    public static void PlayDeathVoice()
    {
        current.voiceSource.clip = current.dethVoiceClip;
        current.voiceSource.Play();

        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();

        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }

    public static void OrbVoice()
    {
        current.voiceSource.clip = current.orbVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.orbFXClip;
        current.fxSource.Play();
    }

    public void BG()
    {
        current.ambientSource.clip = current.AmbientClip;
        current.musicSource.clip = current.musicClip;
        current.ambientSource.loop = true;
        current.musicSource.loop = true;

        current.ambientSource.Play();
        current.musicSource.Play();
    }

    public static void OpenDoorVoice()
    {
        current.fxSource.clip = current.doorOpenClip;
        current.fxSource.Play();
    }
}
