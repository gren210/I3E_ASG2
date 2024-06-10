using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireCooldown;

    private float currentCooldown;

    public float damage;

    public float bulletRange;

    [SerializeField]
    Transform playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = fireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        RaycastHit hitInfo;
        if (currentCooldown <= 0f)
        {
            currentCooldown = fireCooldown;
            if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, bulletRange))
            {
                if (hitInfo.transform.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    Debug.Log("Gun is shot");
                    enemy.enemyHealth -= damage;
                }
            }
        }


    }
}
