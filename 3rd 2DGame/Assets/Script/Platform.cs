using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector3 movement;

    public float speed;

    GameObject top;

    // Start is called before the first frame update
    void Start()
    {
        movement.y = speed;
        top = GameObject.Find("Top");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += movement * Time.deltaTime;
        if (transform.position.y >= top.transform.position.y)
            Destroy(gameObject);
    }
}
