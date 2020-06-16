using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBG : MonoBehaviour
{
    Material ml;

    Vector2 movement;

    public Vector2 speed = -1;

    // Start is called before the first frame update
    void Start()
    {
        ml = GetComponent<Renderer>().material ;
    }

    // Update is called once per frame
    void Update()
    {
        movement += speed;
        ml.mainTextureOffset = movement;
    }
}
