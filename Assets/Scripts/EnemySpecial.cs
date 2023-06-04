using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySpecial : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject viktor;
    [SerializeField] GameObject aleksander;
    [SerializeField] GameObject enemySpecialBulletPrefab;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform gunpoint;
    [SerializeField] Vector3 target1;
    [SerializeField] Vector3 target2;

    [Header("Stats")]
    [SerializeField] float health;
    [SerializeField] float bulletDamage, rotationSpeed, rotationModifier, shootDelay, finalBossHPBuff, finalBossShootBuff;

    public bool canShoot, shootDebuff;

    Vector3 direction;
    private Vector3 target;

    NavMeshAgent agent;
    bool isA;

    void Start()
    {
        HealthCheck();
        canShoot = true;
        target = target2;
        isA = false;
    }

    private void FixedUpdate()
    {
        PlayerCheck();
        if (gameManager.isFinalBoss)
        {
            FinalBossBuff();
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        SetTargetPosition();
        SetAgentPosition();
    }

    IEnumerator ShootingDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    void FinalBossBuff()
    {
        health += finalBossHPBuff;
        shootDelay -= finalBossShootBuff;
    }

    void SetTargetPosition()
    {
        if (agent.remainingDistance == 0)
        {
            if (isA)
            {
                target = target2;
            }
            else
            {
                target = target1;
            }
            isA = !isA;
        }
    }

    void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    private void ShootAtPlayer()
    {
        Instantiate(enemySpecialBulletPrefab, gunpoint.position, transform.rotation);
        Debug.Log("Shooting");
        StartCoroutine(ShootingDelay());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= bulletDamage;
            HealthCheck();
        }
    }

    private void HealthCheck()
    {
        if (health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void PlayerCheck()
    {
        if (!ViktorCheck() && !AleksanderCheck())
        {
            return;
        }
        if (ViktorCheck())
        {
            direction = viktor.transform.position - gameObject.transform.position;
        }
        else if (AleksanderCheck())
        {
            direction = aleksander.transform.position - gameObject.transform.position;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + rotationModifier, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        if (canShoot && !shootDebuff)
        {
            ShootAtPlayer();
        }
    }

    private bool ViktorCheck()
    {
        direction = viktor.transform.position - gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, Mathf.Infinity, layerMask);
        if (hit)
        {
            if (hit.collider.gameObject.name == "Viktor")
            {
                // Debug.Log("Player detected");
                return true;
            }
            else
            {
                // Debug.Log(hit.collider.gameObject.tag);
                return false;
            }
        }
        return false;
    }

    private bool AleksanderCheck()
    {
        direction = aleksander.transform.position - gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, Mathf.Infinity, layerMask);
        if (hit)
        {
            if (hit.collider.gameObject.name == "Aleksander")
            {
                // Debug.Log("Player detected");
                return true;
            }
            else
            {
                // Debug.Log(hit.collider.gameObject.tag);
                return false;
            }
        }
        return false;
    }
}
