using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Enemy : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float chaseSpeed = 0.5f;
    public float directionChangeInterval = 3f;
    public float chaseDistance = 5f; // Distance at which the enemy starts chasing the player
    private Transform playerTransform;
    public bool IsIdle => isIdle;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    private bool isChasing = false;
    private bool isCollidingWithPlayer = false; // Flag for collision with player
    private bool isIdle = false; // Flag for idle state

    // sprite renderer
    SpriteRenderer spriteRenderer;

    // enemy health bar
    private GameObject healthBar;

    public GameObject rock;
    public float rockSpeed = 20f;

    // shoot radius
    private float shootRadius = 5.0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // find player Transform
        playerTransform = GameObject.FindWithTag("Player").transform;

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ChangeDirection());

        //InvokeRepeating("FireRock", 2.0f, 2.0f);
        StartCoroutine(FireRock());


        // get enemy health bar from hierarchy in scene
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

    IEnumerator FireRock()
    {
        while (true) 
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            //Debug.Log("distance: " + distanceToPlayer);
            if (distanceToPlayer <= shootRadius && !isIdle)
            {
                GameObject rockObj = Instantiate(rock, transform.position, Quaternion.identity);
                Physics2D.IgnoreCollision(rockObj.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
                yield return new WaitForSeconds(2.0f);
            }
            else
            {
                // If the player is outside the shoot radius or the enemy is idle, just wait for a short period
                // before checking again, to avoid unnecessary calculations every frame.
                yield return new WaitForSeconds(0.1f);
            }
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

        // get rid of enemy health bar
        Destroy(healthBar);

        // Change the enemy's color to pink
        GetComponent<EnemyFlash>().StopCoroutine(GetComponent<EnemyFlash>().flashCoroutine); //If enemy is flashing, stop flash
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(.97f, 0.51f, 0.48f, 1f); // Using magenta as a stand-in for really pink
        }

        // Optional: Add more code here to change appearance or animations
    }
}
