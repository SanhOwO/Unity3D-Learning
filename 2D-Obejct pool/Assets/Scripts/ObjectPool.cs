using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public GameObject ShadowPrefab;

    public int num;
    //队列Queue，不能检索添加删除 FIFO
    //使用Enqueue()添加到末尾，使用Dequeue()从头取出顺便删除，Peek()在头读取元素，但不删除
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
        //初始对象池
        InitPool();
    }

    //初始化对象池
    public void InitPool()
    { 
        for (int i = 0; i < num; i++)
        {
            var tempShadow = Instantiate(ShadowPrefab);
            tempShadow.transform.SetParent(transform);

            //取消启用，返回对象池
            ReturnPool(tempShadow);
        }
    }

    //返回对象池，等待使用
    public void ReturnPool(GameObject gb)
    {
        gb.SetActive(false);
        availableObjects.Enqueue(gb);
    }

    public GameObject SpawnPool()
    {
        if(availableObjects.Count == 0)
        {
            //不够在加
            InitPool();
        }
        var tempShadow = availableObjects.Dequeue();

        tempShadow.SetActive(true);

        return tempShadow;
    }
}
