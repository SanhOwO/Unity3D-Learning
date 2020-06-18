using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public static CameraControler instance;

    public float speed;

    public Transform target;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        //不希望移动Z轴，所以不变
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), speed * Time.deltaTime);    

    }

    //房间传送过来坐标
    public void ChangeTarget(Transform t)
    {
        target = t;
    }
}
