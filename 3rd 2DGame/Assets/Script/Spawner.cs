using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> plantForm = new List<GameObject>();

    public float spawnTime;

    private float countTime;

    private Vector3 spawnPosition;

    // Update is called once per frame
    void Update()
    {
        SpawnPlatForm();
    }

    public void SpawnPlatForm()
    {
        countTime += Time.deltaTime;
        spawnPosition = transform.position;
        spawnPosition.x = Random.Range(-3f, 3f);

        if(countTime >= spawnTime)
        {
            createPlatForm();
            countTime = 0;
        }
    }

    public void createPlatForm()
    {
        int index = Random.Range(0, plantForm.Count);//random右边不包括 range(0,3) == 0,1,2

        int spikeNum = 0;

        if(index == 3)
        {
            spikeNum++;
        }

        if(spikeNum > 1)
        {
            spikeNum = 0;
            countTime = spawnTime;
            return;
        }

        GameObject newPlat =  Instantiate(plantForm[index], spawnPosition, Quaternion.identity);
        newPlat.transform.SetParent(this.transform);
    }

}
