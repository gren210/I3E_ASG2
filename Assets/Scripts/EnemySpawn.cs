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

    public bool instantDetect;

    Enemy notBoss;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnOverTime)
        {
            if(startSpawning)
            {
                if (currentTimer <= 0)
                {
                    SpawnEnemy();
                    currentTimer = spawnTime;
                }
                currentTimer -= Time.deltaTime;
            }
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyAsset, gameObject.transform.position, gameObject.transform.rotation);
        if (enemy.TryGetComponent<Enemy>(out notBoss))
        {
            if (instantDetect)
            {
                notBoss.detected = true;
            }
        }
        
    }

}
