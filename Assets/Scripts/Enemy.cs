using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject playerTarget;

    [SerializeField]
    float spawnDistance;

    [SerializeField]
    float enemySpeed;

    [SerializeField]
    public float enemyHealth = 100;

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

    bool hasRoared = false;

    [SerializeField]
    float roarSoundDelay;

    float currentRoarTimer;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTimer = damageTimer;
        enemy.speed = enemySpeed;
        playerTarget = GameManager.instance.playerObject;
        currentRoarTimer = 0;

    }


    // Update is called once per frame
    void Update()
    {
        playerTarget = GameManager.instance.playerObject;
        Debug.DrawLine(gameObject.transform.position, playerTarget.transform.position, Color.red);
        NavMeshHit hitInfo;
        bool obstacle = enemy.Raycast(playerTarget.transform.position, out hitInfo);

        if (!obstacle)
        {
            detected = true;
        }

        if (detected)
        {
            currentRoarTimer += Time.deltaTime;
            idleSound.Stop();

            if(!hasRoared)
            {
                detectSound.Play();
                hasRoared = true;
            }

            if (currentRoarTimer > roarSoundDelay)
            {
                roarSound.Play();
                currentRoarTimer = 0;
            }

            currentTimer += Time.deltaTime;
            if (Vector3.Distance(playerTarget.transform.position, transform.position) <= damageDistance)
            {
                enemy.isStopped = true;
                animator.SetBool("Run", false);
                animator.SetBool("Attack", true);
                if (currentTimer >= damageTimer)
                {
                    GameManager.instance.playerHealth -= enemyDamage;
                    GameManager.instance.UpdateHealth();
                    currentTimer = 0;
                }

            }
            else
            {

                enemy.isStopped = false;
                enemy.SetDestination(playerTarget.transform.position);
                animator.SetBool("Run", true);
                animator.SetBool("Attack", false);
            }
        }

        if (enemyHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
