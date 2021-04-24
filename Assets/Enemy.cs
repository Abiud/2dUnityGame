using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public Animator animator;
    public GameObject deathEffect;
    private Transform target;
    public float runSpeed = 2f;
    float moveLimiter = 0.7f;
    public Rigidbody2D rb;
    public float maxRange = 5f;
    public float minRange = .9f;
    private Vector2 originPosition;
    Vector2 movement;
    public int attackDamage = 25;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        originPosition = transform.position;
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= maxRange && Vector2.Distance(target.position, transform.position) >= minRange && currentHealth > 0)
        {
            movement = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y).normalized;
        }
        else if (Vector2.Distance(target.position, transform.position) >= maxRange) 
        {
            if (Vector2.Distance(transform.position, originPosition) >= 0 && Vector2.Distance(transform.position, originPosition) <= 0.5)
            {
                movement = Vector2.zero;
            } 
            else
            {
                movement = new Vector2(originPosition.x - transform.position.x, originPosition.y - transform.position.y).normalized;
            }

        }
        else {
            movement = Vector2.zero;
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("X", movement.x);
        animator.SetFloat("Y", movement.y);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (movement.x != 0 && movement.y != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            movement.x *= moveLimiter;
            movement.y *= moveLimiter;
        }
        rb.velocity = new Vector2(movement.x * runSpeed, movement.y * runSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player") {
            other.collider.GetComponent<PlayerStats>().TakeDamage(attackDamage);
        }
    }

}
