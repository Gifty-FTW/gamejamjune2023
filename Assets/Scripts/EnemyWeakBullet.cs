using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyWeakBullet : MonoBehaviour
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
        string[] collidableTags = { "Bullet", "EWB", "ESB", "Wall", "EnemySpecial", "Player"};
        if (collidableTags.Contains(collision.gameObject.tag))
        {
            Destroy(gameObject);
        }
    }
}
