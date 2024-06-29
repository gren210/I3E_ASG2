using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject playerTarget;

    [SerializeField]
    float spawnDistance;

    [SerializeField]
    float enemySpeed;

    public float enemyHealth = 100;

    public float currentEnemyHealth;

    public GameObject healthBar;

    public Image healthBarGreen;

    bool showHealthBar = false;

    [SerializeField]
    float enemyDamage = 5;

    [SerializeField]
    float damageDistance = 2f;

    [SerializeField]
    float damageTimer = 1f;

    //private GameObject playerObject;

    private float currentTimer;

    //private Player currentPlayer;

    public NavMeshAgent enemy;

    public bool detected;

    public Animator animator;

    [SerializeField]
    AudioSource idleSound;

    [SerializeField]
    AudioSource detectSound;

    [SerializeField]
    AudioSource roarSound;

    [SerializeField]
    AudioSource biteSound;

    bool biteSoundPlayed = false;

    [SerializeField]
    AudioClip deathSound;

    bool hasRoared = false;

    [SerializeField]
    float roarSoundDelay;

    float currentRoarTimer;

    [SerializeField]
    bool attacking = false;

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


    // Update is called once per frame
    void Update()
    {
        playerTarget = GameManager.instance.playerObject;
        Debug.DrawLine(gameObject.transform.position, playerTarget.transform.position, Color.red);
        NavMeshHit hitInfo;
        bool obstacle = enemy.Raycast(playerTarget.transform.position, out hitInfo);

        if(GameManager.instance.hasRestartedCheckpoint)
        {
            Destroy(gameObject);
            //currentEnemyHealth = enemyHealth;
            //enemy.Warp(originalSpawn);
        }

        if (!obstacle)
        {
            detected = true;
        }

        if (detected && !GameManager.instance.hasRestartedCheckpoint)
        {
            currentRoarTimer += Time.deltaTime;
            idleSound.Stop();

            if(!hasRoared)
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
                //biteSound.Stop();
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

        if (currentEnemyHealth <= 0) 
        {
            AudioSource.PlayClipAtPoint(deathSound,gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
