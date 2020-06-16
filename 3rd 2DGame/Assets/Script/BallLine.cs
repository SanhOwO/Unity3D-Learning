using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLine : MonoBehaviour
{
    LineRenderer line;

    public Transform startPoint;
    public Transform endPoint;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);
        
    }
}
