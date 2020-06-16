using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    Animator anim;
    int faderId;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        faderId = Animator.StringToHash("Fade");

        //由于是非静态static，所以手动给gamemanger添加自己
        GameManager.setFader(this);
    }

    //画面切换的动画 //动画分为开始 停止 结束， 自动播发到停止，如果trigger，则播放到停止。
    public void FadeOut()
    {
        anim.SetTrigger(faderId);
    }
}
