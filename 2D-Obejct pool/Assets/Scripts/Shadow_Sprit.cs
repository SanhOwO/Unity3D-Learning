using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow_Sprit : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("Time")]
    public float activeTime;
    public float activeStart;

    [Header("Alpha")]
    private float alpha;
    public float alphaSet;
    public float alphaMultiply;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMultiply;

        color = new Color(1, 1, 1, alpha);

        thisSprite.color = color;

        if(Time.time >= activeStart + activeTime)
        {
            //返回对象池
            ObjectPool.instance.ReturnPool(this.gameObject);
        }
    }
}
