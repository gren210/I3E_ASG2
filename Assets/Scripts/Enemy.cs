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

    bool detected;

    public Animator animator;

    private void Awake()
    {
        //playerTarget = GameManager.instance.playerObject;
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTimer = 1;
        enemy.speed = enemySpeed;
        playerTarget = GameManager.instance.playerObject;

    }


    // Update is called once per frame
    void Update()
    {
        playerTarget = GameManager.instance.playerObject;
        Debug.DrawLine(gameObject.transform.position, playerTarget.transform.position, Color.red);
        //Vector3 targetRay = playerTarget.transform.position - gameObject.transform.position;
        //targetRay.y += 0.5f;
        NavMeshHit hitInfo;
        bool obstacle = enemy.Raycast(playerTarget.transform.position, out hitInfo);
        Debug.Log(hitInfo);

        //Debug.Log(Vector3.Distance(playerTarget.transform.position, transform.position));
        if (!obstacle)// && Vector3.Distance(playerTarget.transform.position, transform.position) <= hostileDistance)
        {
            detected = true;
        }

        if (detected)
        {
            currentTimer += Time.deltaTime;
            if (Vector3.Distance(playerTarget.transform.position, transform.position) <= damageDistance)
            {
                enemy.isStopped = true;
                animator.SetBool("Run", false);
                animator.SetBool("Attack", true);
                if (currentTimer >= damageTimer)
                {
                    GameManager.instance.playerHealth -= enemyDamage;
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
