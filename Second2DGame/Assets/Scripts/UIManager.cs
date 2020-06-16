using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//text mesh pro

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    public TextMeshProUGUI orbText, timeText, deathText,gameOverText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void setOrbText(int o)
    {
        instance.orbText.text = o.ToString();
    }

    public static void setTimeText(float t)
    {
        int minutes = (int)(t / 60);
        float second = t % 60;
        instance.timeText.text = minutes.ToString("00") + ":" + second.ToString("00");//00显示两位
    }

    public static void setDeathText(int d)
    {
        instance.deathText.text = d.ToString();
    }

    public static void DisplayGameOver()
    {
        instance.gameOverText.enabled = true;
    }
}
