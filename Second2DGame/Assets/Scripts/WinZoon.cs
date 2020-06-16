using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZoon : MonoBehaviour
{
    int playerLayer;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            Debug.Log("win");
            GameManager.PlayerWin();
        }
    }

}
