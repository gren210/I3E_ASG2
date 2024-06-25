using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
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

    bool detected;

    public Animator animator;

    bool smash;

    [SerializeField]
    float smashDistance;

    Vector3 currentPlayerLocation;

    [SerializeField]
    GameObject key;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTimer = 0;
        enemy.speed = enemySpeed;
        playerTarget = GameManager.instance.playerObject;

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
            currentTimer += Time.deltaTime;
            if (smash)
            {
                if (currentPlayerLocation == null)
                {
                    currentPlayerLocation = playerTarget.transform.position;
                    enemy.SetDestination(currentPlayerLocation);
                }
                if(currentTimer >= 1.7 && Vector3.Distance(playerTarget.transform.position, transform.position) <= smashDistance)
                {
                    GameManager.instance.playerHealth -= enemyDamage;
                    smash = false;
                }
                if(currentTimer >= damageTimer)
                {
                    smash = false;
                    currentTimer = 0;
                }

            }
            else
            {
                //currentTimer += Time.deltaTime;
                if (Vector3.Distance(playerTarget.transform.position, transform.position) <= damageDistance && currentTimer >= damageTimer)
                {
                    enemy.isStopped = true;
                    animator.SetBool("Run", false);
                    animator.SetTrigger("Attack");
                    if (currentTimer >= damageTimer)
                    {
                        //GameManager.instance.playerHealth -= enemyDamage;
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

        if (enemyHealth <= 0) 
        {
            Destroy(gameObject);
            Instantiate(key, gameObject.transform.position,gameObject.transform.rotation);
        }
    }
}
