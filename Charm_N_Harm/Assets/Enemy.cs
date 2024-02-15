using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseSpeed = 2f;
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

    // enemy health bar
    private GameObject healthBar;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirection());

        // get healthBar from hierarchy of scene
        GameObject child = gameObject.transform.GetChild(1).gameObject;
        healthBar = child.transform.GetChild(0).gameObject;
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
        if (isIdle) return; // Prevent interaction if idle

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

        // get rid of enemy health bar
        Destroy(healthBar);


        // Change the enemy's color to pink
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(.97f, 0.51f, 0.48f, 1f); // Using magenta as a stand-in for really pink
        }

        //Destroy Health Bar
        //var enemyComponent = GetComponent<Enemy>();
        //GameObject child = enemyComponent.transform.GetChild(1).gameObject;
        //child.transform.GetChild(0).gameObject.SetActive(false);

        // Optional: Add more code here to change appearance or animations
    }
}
