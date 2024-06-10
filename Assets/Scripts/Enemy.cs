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

    public float enemyHealth;

    //public NavMeshAgent enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(playerTarget.transform.position, transform.position));
        if (Vector3.Distance(playerTarget.transform.position, transform.position) <= hostileDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, enemySpeed * Time.deltaTime);
            transform.LookAt(playerTarget.transform);
        }

        if(enemyHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
