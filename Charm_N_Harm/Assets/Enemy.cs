using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseSpeed = 2f;
    public float directionChangeInterval = 3f;
    public float chaseDistance = 5f; // Distance at which the enemy starts chasing the player
    public Transform playerTransform;

    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    private bool isChasing = false;
    private bool isCollidingWithPlayer = false; // Flag for collision with player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirection());
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        isChasing = distanceToPlayer <= chaseDistance && !isCollidingWithPlayer; // Check for chase condition

        if (isChasing)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + directionToPlayer * chaseSpeed * Time.fixedDeltaTime);
        }
        else if (!isCollidingWithPlayer) // Move only if not colliding with player
        {
            rb.MovePosition(rb.position + movementPerSecond * Time.fixedDeltaTime);
        }
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            if (!isChasing && !isCollidingWithPlayer) // Change direction only if not chasing and not colliding
            {
                movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                movementPerSecond = movementDirection * moveSpeed;
            }
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true; // Set the flag when colliding with player
        }
        else if (collision.gameObject.tag == "Wall")
        {
            // Change direction when hitting a wall
            movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            movementPerSecond = movementDirection * moveSpeed;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false; // Reset the flag when no longer colliding with player
        }
    }
}
