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

    private GameObject playerObject;

    private float currentTimer;

    private Player currentPlayer;

    private void Awake()
    {
        //playerTarget = GameManager.instance.playerObject;
    }

    private void Start()
    {
        currentTimer = 5;
    }


    // Update is called once per frame
    void Update()
    {
        playerTarget = GameManager.instance.playerObject;
        Debug.DrawLine(gameObject.transform.position, playerTarget.transform.position, Color.red);
        Vector3 targetRay = playerTarget.transform.position - gameObject.transform.position;
        targetRay.y += 0.5f;
        RaycastHit hitInfo;
        Physics.Raycast(gameObject.transform.position, targetRay, out hitInfo, hostileDistance, 01000000);

        //Debug.Log(Vector3.Distance(playerTarget.transform.position, transform.position));
        if(hitInfo.collider != null)
        {
            if (hitInfo.transform.TryGetComponent<Player>(out currentPlayer))// && Vector3.Distance(playerTarget.transform.position, transform.position) <= hostileDistance)
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
        }

        if(enemyHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
