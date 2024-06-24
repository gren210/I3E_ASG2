using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    GameObject enemyAsset;

    public bool startSpawning;

    public bool isSpawnOverTime;

    [SerializeField]
    float spawnTime;

    float currentTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnOverTime)
        {
            if(startSpawning)
            {
                currentTimer += Time.deltaTime;
                if (currentTimer > spawnTime)
                {
                    SpawnEnemy();
                    currentTimer = 0;
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyAsset, gameObject.transform.position, gameObject.transform.rotation);
    }

}
