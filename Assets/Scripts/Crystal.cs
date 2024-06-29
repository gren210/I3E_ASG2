using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Interactable
{
    public string interactText;

    [SerializeField]
    GameObject[] enemies;

    [SerializeField]
    int playMusicIndex;

    [SerializeField]
    int stopMusicIndex;

    [SerializeField]
    Door linkedDoor;

    [SerializeField]
    GameObject enemyToSpawn;

    [SerializeField]
    GameObject checkpointSpawn;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(Player thePlayer)
    {
        base.Interact(thePlayer);
        linkedDoor.CloseDoor();
        GameManager.instance.BGM[stopMusicIndex].Stop();
        GameManager.instance.BGM[playMusicIndex].Play();
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemySpawn>().isSpawnOverTime)
            {
                enemy.GetComponent<EnemySpawn>().startSpawning = true;
            }

            else
            {
                enemy.GetComponent<EnemySpawn>().SpawnEnemy();
                Destroy(enemy);
            }
        }
        Destroy(gameObject);
    }
}
