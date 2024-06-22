using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject playerTarget;

    [SerializeField]
    float hostileDistance;

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

    private GameObject playerObject;

    private float currentTimer;

    private Player currentPlayer;

    public NavMeshAgent enemy;

    bool detected;

    public Animator animator;

    bool isAttacking = false;



    private void Awake()
    {
        //playerTarget = GameManager.instance.playerObject;
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTimer = 1;
    }


    // Update is called once per frame
    void Update()
    {
        playerTarget = GameManager.instance.playerObject;
        Debug.DrawLine(gameObject.transform.position, playerTarget.transform.position, Color.red);
        Vector3 targetRay = playerTarget.transform.position - gameObject.transform.position;
        //targetRay.y += 0.5f;
        NavMeshHit hitInfo;
        bool obstacle = enemy.Raycast(playerTarget.transform.position, out hitInfo);
        Debug.Log(hitInfo);

        //Debug.Log(Vector3.Distance(playerTarget.transform.position, transform.position));
        if (!obstacle)// && Vector3.Distance(playerTarget.transform.position, transform.position) <= hostileDistance)
        {
            detected = true;
            if (Vector3.Distance(playerTarget.transform.position, transform.position) <= damageDistance)
            {
                if (currentTimer >= damageTimer)
                {
                    if (!isAttacking)
                    {
                        animator.SetTrigger("Attack");
                        isAttacking = true;
                    }
                    GameManager.instance.playerHealth -= enemyDamage;
                    currentTimer = 0;
                }
                currentTimer += Time.deltaTime;

            }
            else
            {

            }
        }

        if (detected)
        {
            enemy.SetDestination(playerTarget.transform.position);
            //transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, enemySpeed * Time.deltaTime);
            //transform.LookAt(playerTarget.transform);
        }

        if (enemyHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
