/*
 * Author: Thaqif Adly Bin Mazalan
 * Date: 30/6/24
 * Description: Script for controlling the behavior of a boss enemy in the game, including movement, attacking, and health management.
 */

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : ScriptManager
{
    /// <summary>
    /// The player target the boss will follow and attack.
    /// </summary>
    [SerializeField]
    GameObject playerTarget;

    /// <summary>
    /// The speed of the boss.
    /// </summary>
    [SerializeField]
    float enemySpeed;

    /// <summary>
    /// The total health of the boss.
    /// </summary>
    public float enemyHealth;

    /// <summary>
    /// The current health of the boss.
    /// </summary>
    [HideInInspector]
    public float currentEnemyHealth;

    /// <summary>
    /// The health bar GameObject.
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
    /// The damage the boss does to the player.
    /// </summary>
    [SerializeField]
    float enemyDamage = 5;

    /// <summary>
    /// The distance where the boss can damage the player.
    /// </summary>
    [SerializeField]
    float damageDistance = 2f;

    /// <summary>
    /// The timer before the boss's attacks end.
    /// </summary>
    [SerializeField]
    float damageTimer = 1f;

    /// <summary>
    /// The current timer for tracking attack intervals.
    /// </summary>
    private float currentTimer;

    /// <summary>
    /// The NavMeshAgent component, which is Unity's AI component for movement or pathfinding.
    /// </summary>
    public NavMeshAgent enemy;

    /// <summary>
    /// Bool to check if the player has been detected.
    /// </summary>
    bool detected;

    /// <summary>
    /// The Animator component for controlling the enemy animations.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Bool to check if the boss is doing a smash attack.
    /// </summary>
    bool smash;

    /// <summary>
    /// The distance where the boss does a smash attack.
    /// </summary>
    [SerializeField]
    float smashDistance;

    /// <summary>
    /// The current player location during a smash attack.
    /// </summary>
    GameObject currentPlayerLocation;

    /// <summary>
    /// The key GameObject dropped by the boss upon death.
    /// </summary>
    [SerializeField]
    GameObject key;

    /// <summary>
    /// Bool to check if the boss has attacked.
    /// </summary>
    bool attacked = false;

    /// <summary>
    /// The index of the music to play when the boss is dead.
    /// </summary>
    [SerializeField]
    int playMusicIndex;

    /// <summary>
    /// The audio source for roar sounds.
    /// </summary>
    [SerializeField]
    AudioSource roarSound;

    /// <summary>
    /// The audio source for smash sounds.
    /// </summary>
    [SerializeField]
    AudioSource smashSound;

    /// <summary>
    /// The audio clip for the boss's death sound.
    /// </summary>
    [SerializeField]
    AudioClip deathSound;

    /// <summary>
    /// Bool to check if the boss is chasing the player.
    /// </summary>
    bool isChasing = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTimer = 0;
        enemy.speed = enemySpeed;
        currentEnemyHealth = enemyHealth;
    }

    /// <summary>
    /// Updates the boss's state each frame.
    /// </summary>
    void Update()
    {
        
        playerTarget = GameManager.instance.playerObject;
        float currentDistance = Vector3.Distance(playerTarget.transform.position, transform.position);
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
            currentTimer += Time.deltaTime;
            if (smash)
            {
                // Saves the current player location
                if (currentPlayerLocation == null)
                {
                    currentPlayerLocation = playerTarget;
                    enemy.SetDestination(currentPlayerLocation.transform.position);
                    transform.LookAt(playerTarget.transform);
                }

                // Checks if the plaeyr is in radius when the boss is attacking.
                if (currentTimer >= 1.7f && currentTimer <= 1.7f + Time.deltaTime && currentDistance <= smashDistance && !attacked)
                {
                    GameManager.instance.playerHealth -= enemyDamage;
                    GameManager.instance.UpdateHealth();
                    attacked = true;
                    smashSound.Play();
                }

                // Resets the boss 
                if (currentTimer >= damageTimer)
                {
                    smash = false;
                    currentTimer = 0;
                    attacked = false;
                    currentPlayerLocation = null;
                    isChasing = false;
                }
            }
            else
            {

                // plays the roar sound
                if (!isChasing)
                {
                    roarSound.Play();
                    isChasing = true;
                }

                // Checks if the boss is withing radius to initiate the smash attack
                if (currentDistance <= damageDistance && currentTimer >= damageTimer)
                {
                    enemy.isStopped = true;
                    animator.SetBool("Run", false);
                    animator.SetTrigger("Attack");
                    // ensures the Boss does not continuously attack the player after smashing.
                    if (currentTimer >= damageTimer)
                    {
                        currentTimer = 0;
                        smash = true;
                    }
                }
                else
                {
                    currentTimer = damageTimer;
                    enemy.isStopped = false;
                    enemy.SetDestination(playerTarget.transform.position);
                    animator.SetBool("Run", true);
                }
            }
        }

        // Runs when the boss is damaged, showing the health bar.
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

        // Runs when the boss dies.
        if (currentEnemyHealth <= 0)
        {
            KillEnemy(deathSound, gameObject);
        }
    }

    /// <summary>
    /// Function for when the boss is killed.
    /// </summary>
    /// <param name="sound">The sound to play when the boss dies.</param>
    /// <param name="enemy">The enemy GameObject to destroy.</param>
    protected override void KillEnemy(AudioClip sound, GameObject enemy)
    {
        GameManager.instance.objectiveText.text = GameManager.instance.objectiveStrings[8];
        Instantiate(key, enemy.transform.position, enemy.transform.rotation);
        ChangeMusic(playMusicIndex);
        base.KillEnemy(sound, enemy);
    }
}

