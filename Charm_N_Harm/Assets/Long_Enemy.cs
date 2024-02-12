using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Enemy : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float chaseSpeed = 0.5f;
    public float directionChangeInterval = 3f;
    public float chaseDistance = 5f; // Distance at which the enemy starts chasing the player
    public Transform playerTransform;
    public bool IsIdle => isIdle;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    private bool isChasing = false;
    private bool isCollidingWithPlayer = false; // Flag for collision with player
    private bool isIdle = false; // Flag for idle state

    public GameObject rock;
    public float rockSpeed = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirection());

        InvokeRepeating("FireRock", 5.0f, 5.0f);
    }

    void FixedUpdate()
    {
        if (isIdle) return; // Skip movement logic if idle

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        isChasing = distanceToPlayer <= chaseDistance && !isCollidingWithPlayer;

        if (isChasing)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + directionToPlayer * chaseSpeed * Time.fixedDeltaTime);
        }
        else if (!isCollidingWithPlayer)
        {
            rb.MovePosition(rb.position + movementPerSecond * Time.fixedDeltaTime);
        }

    }

    void FireRock()
    {
        Vector2 pos = transform.position;
        Vector2 right = transform.right * 1.45f;
        pos = pos + right;

        GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
        //Rigidbody2D rockRb = rockObj.GetComponent<Rigidbody2D>();
        //Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        ////rockRb.velocity = (directionToPlayer * rockSpeed);
        //Debug.Log(rockRb.velocity);
        ////rockRb.velocity = new Vector2(1, 1) * rockSpeed;
        //rockRb.AddForce(new Vector2(1, 1) * rockSpeed);
        //Debug.Log(rockRb.velocity);
    }


    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            if (!isChasing && !isCollidingWithPlayer && !isIdle)
            {
                movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                movementPerSecond = movementDirection * moveSpeed;
            }
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isIdle)
        {
            Debug.Log("long enemy is idle");
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            movementPerSecond = movementDirection * moveSpeed;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }

    public void SetToIdle()
    {
        isIdle = true;
        rb.velocity = Vector2.zero; // Stop any movement immediately

        // Change the enemy's color to pink
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(.97f, 0.51f, 0.48f, 1f); // Using magenta as a stand-in for really pink
        }

        // Optional: Add more code here to change appearance or animations
    }
}
