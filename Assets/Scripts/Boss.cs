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

    GameObject currentPlayerLocation;

    [SerializeField]
    GameObject key;

    bool attacked = false;

    [SerializeField]
    int playMusicIndex;

    [SerializeField]
    int stopMusicIndex;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTimer = 0;
        enemy.speed = enemySpeed;
    }


    // Update is called once per frame
    void Update()
    {
        playerTarget = GameManager.instance.playerObject;
        float currentDistance = Vector3.Distance(playerTarget.transform.position, transform.position);
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
                    currentPlayerLocation = playerTarget;
                    enemy.SetDestination(currentPlayerLocation.transform.position);
                    transform.LookAt(playerTarget.transform);
                }
                if(currentTimer >= 1.7 && currentTimer <= 1.7 + Time.deltaTime && currentDistance <= smashDistance && !attacked)                
                {
                    GameManager.instance.playerHealth -= enemyDamage;
                    GameManager.instance.UpdateHealth();
                    attacked = true;
                }
                if(currentTimer >= damageTimer)
                {
                    smash = false;
                    currentTimer = 0;
                    attacked = false;
                    currentPlayerLocation = null;
                    Debug.Log("bruh");
                }

            }
            else
            {
                //currentTimer += Time.deltaTime;
                if (currentDistance <= damageDistance && currentTimer >= damageTimer)
                {
                    enemy.isStopped = true;
                    animator.SetBool("Run", false);
                    animator.SetTrigger("Attack");
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

        if (enemyHealth <= 0) 
        {
            GameManager.instance.BGM[stopMusicIndex].Stop();
            GameManager.instance.BGM[playMusicIndex].Play();
            Destroy(gameObject);
            Instantiate(key, gameObject.transform.position,gameObject.transform.rotation);
        }
    }
}
