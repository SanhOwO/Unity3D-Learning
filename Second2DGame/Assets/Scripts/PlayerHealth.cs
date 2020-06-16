using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    int spikesLayer;

    // Start is called before the first frame update
    void Start()
    {
        spikesLayer = LayerMask.NameToLayer("spikes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == spikesLayer)
        {
            //在position出生成VFXPrefab
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);
            AudioManager.PlayDeathVoice();
            gameObject.SetActive(false);

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.PlayerDie();
        }
    }
}
