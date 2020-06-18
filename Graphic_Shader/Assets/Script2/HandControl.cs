using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class HandControl : MonoBehaviour
{
    //public SpriteResolver hand;
    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();
    SpriteResolver handResolver;
    public GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        //获得所有接收器
        foreach (var rev in FindObjectsOfType<SpriteResolver>())
        {
            spriteResolvers.Add(rev);
            if(rev.GetCategory() == "hand")
            {
                handResolver = rev;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
           // Debug.Log("press E");
            foreach (var rev in FindObjectsOfType<SpriteResolver>())
            {
                rev.SetCategoryAndLabel(rev.GetCategory(), "book");
            }
            if(handResolver.GetLabel() == "book")
            {
                Debug.Log("现在book已经出来");
                //item.SetActive(true);
            }
        }
    }
}
