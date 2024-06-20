using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float currentTimer;

    private void Awake()
    {
        playerTarget = GameManager.instance.player;
    }

    private void Start()
    {
        currentTimer = 5;
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(playerTarget.transform.position, transform.position));
        if (Vector3.Distance(playerTarget.transform.position, transform.position) <= hostileDistance)
        {
            if (Vector3.Distance(playerTarget.transform.position, transform.position) <= damageDistance)
            {
                if (currentTimer >= damageTimer)
                {
                    GameManager.instance.playerHealth -= enemyDamage;
                    currentTimer = 0;
                }
                currentTimer += Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, enemySpeed * Time.deltaTime);
                transform.LookAt(playerTarget.transform);
            }
        }

        if(enemyHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
