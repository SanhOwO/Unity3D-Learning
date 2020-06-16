using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int player;
    public GameObject explosionVFX;

    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.NameToLayer("Player");

        GameManager.setOrbs(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == player)
        {
            Instantiate(explosionVFX, transform.position, transform.rotation);
            
            gameObject.SetActive(false);

            AudioManager.OrbVoice();

            GameManager.removeOrbs(this);
        }
    }


}
