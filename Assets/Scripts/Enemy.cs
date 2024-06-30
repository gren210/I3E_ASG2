/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: This enemy script controls the enemy's movement and behaviour.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : ScriptManager
{
    /// <summary>
    /// The player target the enemy will follow and attack.
    /// </summary>
    [SerializeField]
    GameObject playerTarget;

    /// <summary>
    /// The speed of the enemy.
    /// </summary>
    [SerializeField]
    float enemySpeed;

    /// <summary>
    /// The total health of the enemy.
    /// </summary>
    public float enemyHealth = 100;

    /// <summary>
    /// The current health of the enemy.
    /// </summary>
    public float currentEnemyHealth;

    /// <summary>
    /// The health bar of the enemy.
    /// </summary>
    public GameObject healthBar;

    /// <summary>
    /// The UI image showing the health bar's green portion.
    /// </summary>
    public Image healthBarGreen;

    /// <summary>
    /// Bool to check if the health bar should be shown.
    /// </summary>
    bool showHealthBar = false;

    /// <summary>
    /// The damage the enemy deals to the player.
    /// </summary>
    [SerializeField]
    float enemyDamage = 5;

    /// <summary>
    /// The distance where the enemy can damage the player.
    /// </summary>
    [SerializeField]
    float damageDistance = 2f;

    /// <summary>
    /// The timer between enemy attacks.
    /// </summary>
    [SerializeField]
    float damageTimer = 1f;

    /// <summary>
    /// The current timer for the attacks.
    /// </summary>
    private float currentTimer;

    /// <summary>
    /// The NavMeshAgent component which is Unity's AI for object movement or pathfinding. 
    /// </summary>
    public NavMeshAgent enemy;

    /// <summary>
    /// Bool to check if the player has been detected.
    /// </summary>
    public bool detected;

    /// <summary>
    /// The Animator component for controlling animations.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// The audio source for idle sounds.
    /// </summary>
    [SerializeField]
    AudioSource idleSound;

    /// <summary>
    /// The audio source for detection sounds.
    /// </summary>
    [SerializeField]
    AudioSource detectSound;

    /// <summary>
    /// The audio source for roar sounds.
    /// </summary>
    [SerializeField]
    AudioSource roarSound;

    /// <summary>
    /// The audio source for bite sounds.
    /// </summary>
    [SerializeField]
    AudioSource biteSound;

    /// <summary>
    /// Bool to check if the bite sound has been played.
    /// </summary>
    bool biteSoundPlayed = false;

    /// <summary>
    /// The audio clip for the enemy's death sound.
    /// </summary>
    [SerializeField]
    AudioClip deathSound;

    /// <summary>
    /// Bool to check if the enemy has roared.
    /// </summary>
    bool hasRoared = false;

    /// <summary>
    /// The delay before the enemy roars.
    /// </summary>
    [SerializeField]
    float roarSoundDelay;

    /// <summary>
    /// The current timer for tracking roar intervals.
    /// </summary>
    float currentRoarTimer;

    /// <summary>
    /// Bool to check if the enemy is attacking.
    /// </summary>
    [SerializeField]
    bool attacking = false;

    /// <summary>
    /// The original spawn position of the enemy.
    /// </summary>
    Vector3 originalSpawn;

    private void Start()
    {
        originalSpawn = transform.position;
        animator = gameObject.GetComponent<Animator>();
        currentTimer = damageTimer;
        enemy.speed = enemySpeed;
        playerTarget = GameManager.instance.playerObject;
        currentRoarTimer = 0;
        currentEnemyHealth = enemyHealth;
    }


    void Update()
    {
        // Constantly checks if the player is within line of sight.
        playerTarget = GameManager.instance.playerObject;
        NavMeshHit hitInfo;
        bool obstacle = enemy.Raycast(playerTarget.transform.position, out hitInfo);

        if (!obstacle)
        {
            detected = true;
        }

        // Run when the player is detected and theres no obstacles between the player and enemy.
        if (detected)
        {
            currentRoarTimer += Time.deltaTime;
            idleSound.Stop();

            // If blocks to control the enemy sounds
            if (!hasRoared)
            {
                detectSound.Play();
                hasRoared = true;
            }

            if (currentRoarTimer > roarSoundDelay && !attacking)
            {
                roarSound.Play();
                currentRoarTimer = 0;
            }

            currentTimer += Time.deltaTime;

            // Enemy attacks the player when it is close enough.
            if (Vector3.Distance(playerTarget.transform.position, transform.position) <= damageDistance)
            {
                attacking = true;
                roarSound.Stop();
                detectSound.Stop();
                if (!biteSoundPlayed)
                {
                    biteSound.Play();
                    biteSoundPlayed = true;
                }
                enemy.isStopped = true;
                animator.SetBool("Run", false);
                animator.SetBool("Attack", true);
                if (currentTimer >= damageTimer && !GameManager.instance.isImmune)
                {
                    GameManager.instance.playerHealth -= enemyDamage;
                    GameManager.instance.UpdateHealth();
                    currentTimer = 0;
                }
            }
            else
            {
                if (biteSoundPlayed)
                {
                    roarSound.Play();
                    biteSound.Stop();
                }
                attacking = false;
                biteSoundPlayed = false;
                enemy.isStopped = false;
                enemy.SetDestination(playerTarget.transform.position);
                animator.SetBool("Run", true);
                animator.SetBool("Attack", false);
            }
        }

        // Run when the enemy takes damage, showing its health bar.
        if (currentEnemyHealth < enemyHealth)
        {
            if (!showHealthBar)
            {
                showHealthBar = true;
                healthBar.SetActive(true);
                healthBarGreen.fillAmount = currentEnemyHealth / enemyHealth;
            }
            healthBar.transform.LookAt(healthBar.transform.position + GameManager.instance.playerCamera.forward);
        }

        // Run when the enemy dies.
        if (currentEnemyHealth <= 0)
        {
            KillEnemy(deathSound, gameObject);
        }
    }
}

