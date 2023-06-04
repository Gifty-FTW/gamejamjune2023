using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Viktor : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunpoint;
    [SerializeField] GameObject gunImg;
    [SerializeField] Image healthBar;
    [SerializeField] Image sanityBar;
    [SerializeField] Text bulletCount;
    [SerializeField] GameObject HUD;

    [Header("Stats")]
    [SerializeField] float health;
    [SerializeField] float sanity, sanityDrainRate, speed, shootSpeed, weakDamage, reloadSpeed, buffDuration, buffSanityRestoration, shootSpeedBuff, specialDamage;
    [SerializeField] float finalBossWeakDamageDebuff, finalBossSpecialDamageDebuff;
    [SerializeField] int totalAmmo, currentAmmo;

    Rigidbody2D rb;

    Vector2 moveDirection;

    float moveX, moveY, sanityMax;

    bool canShoot;
    public bool canMove;

    SpriteRenderer gunSprite;
    

    void Start()
    {
        canShoot = true;
        rb = gameObject.GetComponent<Rigidbody2D>();

        sanityMax = sanity;

        gunSprite = gunImg.GetComponent<SpriteRenderer>();

        UpdateHealth();
        UpdateSanity();
        UpdateBulletCount();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot && currentAmmo > 0 && sanity > 0)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadingDelay());
        }
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
            if (moveX != 0 || moveY != 0)
            {
                Rotate();
            }
        }
        if (gameManager.isFinalBoss)
        {
            FinalBossDebuff();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HP")
        {
            health += 10.0f;
            Mathf.Clamp(health, 0.0f, 100.0f);
            UpdateHealth();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "BP")
        {
            totalAmmo += 15;
            UpdateBulletCount();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "EWB")
        {
            health -= weakDamage;
            UpdateHealth();
        }
        if (collision.gameObject.tag == "ESB")
        {
            health -= specialDamage;
            UpdateHealth();
        }
    }

    IEnumerator DisableShooting()
    {
        canShoot = false;
        gunSprite.color = new Color(1, 1, 1, 0.4f);
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
        gunSprite.color = new Color(1, 1, 1, 1.0f);
    }

    public void Buff()
    {
        sanity += buffSanityRestoration;
        sanity = Mathf.Clamp(sanity, 0.0f, sanityMax);
        UpdateSanity();
        StartCoroutine(ShootBuff());
    }

    IEnumerator ShootBuff()
    {
        shootSpeed -= shootSpeedBuff;
        yield return new WaitForSeconds(buffDuration);
        shootSpeed += shootSpeedBuff;
    }

    IEnumerator ReloadingDelay()
    {
        yield return new WaitForSeconds(reloadSpeed);
        Reload();
    }

    void FinalBossDebuff()
    {
        weakDamage -= finalBossWeakDamageDebuff;
        specialDamage -= finalBossSpecialDamageDebuff;
    }

    public void Disable()
    {
        canMove = false;
        rb.bodyType = RigidbodyType2D.Static;
        HUD.SetActive(false);
    }

    public void Enable()
    {
        canMove = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        HUD.SetActive(true);
    }

    private void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY);
    }

    private void Move()
    {
        rb.velocity = moveDirection.normalized * speed; // new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    private void Rotate()
    {
        float angle = Mathf.Atan2(-moveX, moveY) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Reload()
    {
        int missingAmmo = 15 - currentAmmo;
        totalAmmo -= missingAmmo;
        if (totalAmmo <= 0)
        {
            if (totalAmmo <= 15)
            {
                if (currentAmmo <= 0)
                {
                    bulletCount.text = "No Ammo!";
                }
                else
                {
                    totalAmmo = 0;
                    UpdateBulletCount();
                }
            }
            else
            {
                missingAmmo += totalAmmo;
                UpdateBulletCount();
            }
        }
        else
        {
            currentAmmo += missingAmmo;
            UpdateBulletCount();
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, gunpoint.position, transform.rotation);
        currentAmmo--;                                                                      
        UpdateBulletCount();
        sanity -= sanityDrainRate;
        UpdateSanity();
        StartCoroutine(DisableShooting());
    }

    private void UpdateHealth()
    {
        if (health <= 0)
        {
            ViktorGG();
        }
        healthBar.fillAmount = health / 100.0f;
    }

    private void UpdateSanity()
    {
        sanityBar.fillAmount = sanity / sanityMax;
        shootSpeed = 0.75f + 0.05f * (sanityMax - sanity);
        reloadSpeed = 1.0f + 0.025f * (sanityMax - sanity);
    }

    private void UpdateBulletCount()
    {
        bulletCount.text = "Ammo = " + currentAmmo.ToString() + "/" + totalAmmo.ToString();
    }

    private void ViktorGG()
    {

    }
}
