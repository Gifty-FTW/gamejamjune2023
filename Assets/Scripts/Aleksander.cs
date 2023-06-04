using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aleksander : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] GameManager gameManager;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject HUD;

    [Header("Stats")]
    [SerializeField] float health;
    [SerializeField] float weakDamage, speed, specialDamage;
    [SerializeField] float finalBossWeakDamageDebuff, finalBossSpecialDamageDebuff;
    
    public bool canMove;

    Rigidbody2D rb;

    Vector2 moveDirection;

    float moveX, moveY;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        UpdateHealth();
    }

    void Update()
    {
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

    private void UpdateHealth()
    {
        if (health <= 0)
        {
            AleksanderGG();
        }
        healthBar.fillAmount = health / 100.0f;
    }

    private void AleksanderGG()
    {

    }
}
