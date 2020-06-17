using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_System : MonoBehaviour
{
    [Header("UI")]
    public Text textLable;
    public Image img;

    [Header("document")]
    public TextAsset textFile;
    public int index;

    [Header("Icon")]
    public Sprite face1;
    public Sprite face2;

    public float textSpeed=0.1f;
    float tempTextSpeed;

    bool textIsRuning = false;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        SplitText(textFile);
    }
    private void OnEnable()
    {
        //textLable.text = textList[index];
        //index++;
        tempTextSpeed = textSpeed;
        index = 0;
        StartCoroutine(SetTextUI());
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(textIsRuning == true)
            {
                tempTextSpeed = 0.01f;
            }
            else
            {
                tempTextSpeed = textSpeed;
                if (index >= textList.Count)
                {
                    index = 0;
                    gameObject.SetActive(false);
                    return;
                }
                //textLable.text = textList[index];
                //index++;           
                StartCoroutine(SetTextUI());
            }
        }
    }

    void SplitText(TextAsset file)
    {
        textList.Clear();
        index = 0;

        //var 可替换任何类型变量
        var lineData = file.text.Split('\n');

        textList.AddRange(lineData);
        /* foreach (var i in lineData) 
         {
             textList.Add(i);
         }*/

    }
    
    //在执行IEnumerato时，所有yield return后面的内容都会被暂停 挂起
    //经过测试，yield 里的时间是动态可以在运行时被改变
    IEnumerator SetTextUI()
    {
        textLable.text = "";
        textIsRuning = true;
        
        switch(textList[index])
        {
            case"A":
                img.sprite = face1;
                index++;
                break;
            case"B":
                img.sprite = face2;
                index++;
                break;
        }
        


        for (int i = 0; i < textList[index].Length; i++)
        {
            textLable.text += textList[index][i];

            //固定写法
            yield return new WaitForSeconds(tempTextSpeed);
        }
        textIsRuning = false; 
        index++;
    }
}
