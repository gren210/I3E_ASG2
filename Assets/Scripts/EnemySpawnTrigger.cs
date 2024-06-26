using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject[] enemies;

    [SerializeField]
    bool instantDetect;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemySpawn>().isSpawnOverTime)
                {
                    enemy.GetComponent<EnemySpawn>().startSpawning = true;
                }

                else
                {
                    enemy.GetComponent<EnemySpawn>().SpawnEnemy();
                    Destroy(enemy,0.1f);
                }
                if (instantDetect)
                {
                    enemy.GetComponent<EnemySpawn>().instantDetect = true;
                }
            }
            Destroy(gameObject);
        }
    }
}
