using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWeak : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject viktor;
    [SerializeField] GameObject aleksander;
    [SerializeField] GameObject enemyWeakBulletPrefab;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform gunpoint;

    [Header("Stats")]
    [SerializeField] float health;
    [SerializeField] float bulletDamage;
    [SerializeField] float rotationSpeed, rotationModifier, shootDelay, shootDebuffDuration, finalBossHPBuff, finalBossShootBuff;
    
    public bool canShoot, shootDebuff;

    Vector3 direction;

    void Start()
    {
        HealthCheck();
        canShoot = true;
    }

    private void FixedUpdate()
    {
        PlayerCheck();
        if (gameManager.isFinalBoss)
        {
            FinalBossBuff();
        }
    }

    IEnumerator ShootingDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    IEnumerator ShootDebuff()
    {
        shootDebuff = true;
        yield return new WaitForSeconds(shootDebuffDuration);
        shootDebuff = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= bulletDamage;
            HealthCheck();
        }
    }

    void FinalBossBuff()
    {
        health += finalBossHPBuff;
        shootDelay -= finalBossShootBuff;
    }
    private void HealthCheck()
    {
        if (health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Debuff()
    {
        StartCoroutine(ShootDebuff());
    }

    private void ShootAtPlayer()
    {
        Instantiate(enemyWeakBulletPrefab, gunpoint.position, transform.rotation);
        StartCoroutine(ShootingDelay());
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
                return true;
            }
            else
            {
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
