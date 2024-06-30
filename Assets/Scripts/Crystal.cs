/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Handles the functionality of a crystal
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Interactable
{
    /// <summary>
    /// Array of enemies that spawn upon interaction with this crystal.
    /// </summary>
    [SerializeField]
    GameObject[] enemies;

    /// <summary>
    /// Music to play when the crystal is interacted with.
    /// </summary>
    [SerializeField]
    int playMusicIndex;

    /// <summary>
    /// Music to stop when the crystal is interacted with.
    /// </summary>
    [SerializeField]
    int stopMusicIndex;

    /// <summary>
    /// Door that closes upon interaction with the crystal.
    /// </summary>
    [SerializeField]
    Door linkedDoor;

    /// <summary>
    /// Enemy to spawn when the crystal is interacted with.
    /// </summary>
    [SerializeField]
    GameObject enemyToSpawn;


    /// <summary>
    /// Handles interaction with the player.
    /// </summary>
    /// <param name="thePlayer">The player interacting with the crystal.</param>
    public override void Interact(Player thePlayer)
    {
        GameManager.instance.objectiveText.text = GameManager.instance.objectiveStrings[7];
        base.Interact(thePlayer);

        // Close the linked door
        linkedDoor.CloseDoor();

        // Change the background music
        ChangeMusic(playMusicIndex);

        // Spawn or activate enemies
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

        // Destroy the crystal after interaction
        Destroy(gameObject);
    }
}
