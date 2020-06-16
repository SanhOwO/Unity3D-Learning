using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //int orbNum;
    int deathNum;
    //方便其他clss引用GameManger
    static GameManager instance;

    SceneFader scenefader;

    List<Orb> orbs;

    WinDoor door;

    UIManager UI;

    float gameTime;

    bool gameisOver;
    private void Update()
    {
        if (gameisOver)
        {
            return;
        }
        //UIManager.setDeathText(instance.deathNum);
        //UIManager.setDeathText(instance.orbNum);
        UIManager.setOrbText(instance.orbs.Count);
        UIManager.setDeathText(instance.deathNum);

        gameTime += Time.deltaTime;
        UIManager.setTimeText(instance.gameTime);
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);

        orbs = new List<Orb>();
        door = new WinDoor();
        UI = new UIManager();
        deathNum = 0;
    }

    public static void PlayerDie()
    {
        instance.deathNum++;
        //延时播放
        //SceneFader.FadeOut(); 这里由于SceneFader这个class不是static静态，所以不能被调用,所以设置了一个setter给他让他自己添加过来
        instance.scenefader.FadeOut();
        
        instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        instance.orbs.Clear();
    }

    public static void setFader(SceneFader sf)
    {
        instance.scenefader = sf;
    }

    public static void setOrbs(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
        {
            instance.orbs.Add(orb);
        }
        //UIManager.setOrbText(instance.orbs.Count);
    }

    public static void removeOrbs(Orb orb)
    {
        if (instance.orbs.Contains(orb))
        {
            instance.orbs.Remove(orb);
        }
        if (collectAll())
        {
            instance.door.Open();
        }
        //UIManager.setOrbText(instance.orbs.Count);
    }

    public static void setDoor(WinDoor d)
    {
        instance.door = d;
    }
    
    public static bool collectAll()
    {
        if(instance.orbs.Count == 0)
        {
            return true;
        }
        return false;
    }

    public static void PlayerWin()
    {
        instance.gameisOver = true;
        UIManager.DisplayGameOver();
    }

    public static bool GameOver()
    {
        return instance.gameisOver;
    }
}
