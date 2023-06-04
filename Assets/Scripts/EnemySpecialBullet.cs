using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpecialBullet : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string[] collidableTags = { "Bullet", "EWB", "ESB", "Wall", "EnemyWeak", "Player" };
        if (collidableTags.Contains(collision.gameObject.tag))
        {
            Destroy(gameObject);
        }
    }
}
