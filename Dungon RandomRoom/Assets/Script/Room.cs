using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public GameObject up, down, left, right;
    public bool hasUp, hasDown, hasLeft, hasRight;

    public int stepToStart;

    public Text text;

    public int doorNum;
    // Start is called before the first frame update
    void Start()
    {
        up.SetActive(hasUp);
        down.SetActive(hasDown);
        left.SetActive(hasLeft);
        right.SetActive(hasRight);
    }

    public void UpdateRoom(float x, float y)
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / x) + Mathf.Abs(transform.position.y / y));

        text.text = stepToStart.ToString();

        //判断一道门
        if (hasUp)
            doorNum++;
        if (hasDown)
            doorNum++;
        if (hasLeft)
            doorNum++;
        if (hasRight)
            doorNum++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(transform.position + "  ---");
        if (collision.CompareTag("Player"))
        {
            CameraControler.instance.ChangeTarget(transform);
        }
    }
}
