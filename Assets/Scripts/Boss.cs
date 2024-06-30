using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : ScriptManager
{
    [SerializeField]
    GameObject playerTarget;

    [SerializeField]
    float enemySpeed;

    public float enemyHealth;

    [HideInInspector]
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

    private float currentTimer;

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

    [SerializeField]
    AudioSource roarSound;

    [SerializeField]
    AudioSource smashSound;

    [SerializeField]
    AudioClip deathSound;

    bool isChasing = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTimer = 0;
        enemy.speed = enemySpeed;
        currentEnemyHealth = enemyHealth;
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
                    smashSound.Play();
                }
                if(currentTimer >= damageTimer)
                {
                    smash = false;
                    currentTimer = 0;
                    attacked = false;
                    currentPlayerLocation = null;
                    isChasing = false;
                }

            }
            else
            {
                if (!isChasing)
                {
                    roarSound.Play();
                    isChasing = true;
                }
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
            KillEnemy(deathSound, gameObject);
        }
    }

    protected override void KillEnemy(AudioClip sound, GameObject enemy)
    {
        GameManager.instance.objectiveText.text = GameManager.instance.objectiveStrings[8];
        Instantiate(key, enemy.transform.position, enemy.transform.rotation);
        ChangeMusic(playMusicIndex);
        base.KillEnemy(sound, enemy);

    }
}
