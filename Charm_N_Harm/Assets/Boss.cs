using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boss : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseSpeed = 0.3f;
    public float directionChangeInterval = 3f;
    public float chaseDistance = 5f; // Distance at which the enemy starts chasing the player
    public Transform playerTransform;
    public bool IsIdle => isIdle;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    //private bool isChasing = false;
    private bool isCollidingWithPlayer = false; // Flag for collision with player
    private bool isIdle = false; // Flag for idle state
    private bool isChasing = false;

    // enemy health bar
    private GameObject healthBar;

    // player transform
    private Transform player;

    // rock prefab and enemy prefab
    public GameObject rock;
    public GameObject enemy;

    // some new variables for movements
    public float actionInterval = 2.0f;
    public float dashDuration = 5.0f;
    public float shootInterval = 1.0f;
    public float dashCoolDown = 3.0f;
    public float spawnInterval = 10.0f;

    private bool isDashing;

    // tilemap, used for checking if enemy is spawning on our tilemap or not
    public Tilemap tilemap;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BossBehaviorCycle());

        // find player Transform
        player = GameObject.FindWithTag("Player").transform;

        //get healthBar from hierarchy of scene
        GameObject child = gameObject.transform.GetChild(1).gameObject;
        healthBar = child.transform.GetChild(0).gameObject;

        // trying to prevent boss from easily tipping over
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (isIdle) return; // Skip movement logic if idle

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        isChasing = distanceToPlayer <= chaseDistance && !isCollidingWithPlayer;

        if (isChasing && !isDashing)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + directionToPlayer * chaseSpeed * Time.fixedDeltaTime);
        }
        //else if (!isCollidingWithPlayer)
        //{
        //    rb.MovePosition(rb.position + movementPerSecond * Time.fixedDeltaTime);
        //}
    }

    IEnumerator BossBehaviorCycle()
    {
        while (true) // Keep cycling through behaviors
        {
            // Shoot rocks
            yield return StartCoroutine(ShootRocks());
            yield return new WaitForSeconds(actionInterval); // Wait between actions

            // Dash towards player
            yield return StartCoroutine(DashTowardsPlayer());
            yield return new WaitForSeconds(actionInterval); // Wait between actions

            // Spawn enemies
            yield return StartCoroutine(SpawnEnemies());
            yield return new WaitForSeconds(actionInterval); // Wait between actions
        }
    }

    private IEnumerator ShootRocks()
    {
        //while (true)
        //{
        //    if (!isCollidingWithPlayer && !isIdle)
        //    {
        //        Vector2 pos = transform.position;

        //        GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
        //        Physics2D.IgnoreCollision(rockObj.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
        //    }
        //    yield return new WaitForSeconds(shootInterval);

        //}
        chaseSpeed = 0.5f;
        isChasing = true;
        int count = 3;
        for (var i = 0; i < count; i++)
        {
            Vector2 pos = transform.position;
            GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
            Physics2D.IgnoreCollision(rockObj.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
            yield return new WaitForSeconds(shootInterval);
        }

    }

    private IEnumerator DashTowardsPlayer()
    {
        //float startTime = Time.time;
        //Vector2 startPosition = transform.position;
        //Vector2 endPosition = player.position; // Assuming you have a reference to the player's transform

        //while (Time.time < startTime + dashDuration)
        //{
        //    // Move from start to end position over dashDuration
        //    transform.position = Vector2.Lerp(startPosition, endPosition, (Time.time - startTime) / dashDuration);
        //    yield return null; // Wait for a frame
        //}

        //yield return new WaitForSeconds(dashCoolDown); // Cooldown before dashing again
        //chaseSpeed = 5.0f;
        //isChasing = true;
        //yield return new WaitForSeconds(dashCoolDown);
        isDashing = true; // Assume isDashing is a new variable you've declared
        float originalSpeed = chaseSpeed;
        chaseSpeed = 5.0f; // Set dash speed

        Vector2 start = transform.position;
        Vector2 target = player.position;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            // Example dash logic, replace with your intended behavior
            //transform.position = Vector2.Lerp(start, target, (Time.time - startTime) / dashDuration);
            //yield return null;
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + directionToPlayer * chaseSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        chaseSpeed = originalSpeed; // Reset speed after dash
        isDashing = false; // Reset dashing flag
        yield return new WaitForSeconds(dashCoolDown);
    }

    private IEnumerator SpawnEnemies()
    {
        //isChasing = false;
        //chaseSpeed = 0.5f;
        Vector3 pos = transform.position;
        Vector3 right = transform.right * 3.0f;
        Vector3 pos_right = pos + right;

        // check if this is in tile map
        Vector3Int cellPosition = tilemap.WorldToCell(pos_right);
        if (tilemap.HasTile(cellPosition))
        {
            Instantiate(enemy, pos_right, Quaternion.identity);
            EnemyManager.Instance.enemyCount++; // Directly increment the count
        }
        else
        {
            yield return new WaitForSeconds(spawnInterval);
        }
        

        Vector3 left = -(transform.right * 3.0f);
        Vector3 pos_left = pos + left;
        // check if this is in tile map
        Vector3Int again = tilemap.WorldToCell(pos_left);
        if (tilemap.HasTile(again))
        {
            Instantiate(enemy, pos_left, Quaternion.identity);
            EnemyManager.Instance.enemyCount++; // Directly increment the count
        }
        else
        {
            yield return new WaitForSeconds(spawnInterval);
        }
        //Instantiate(enemy, pos_left, Quaternion.identity);

        yield return new WaitForSeconds(spawnInterval); // Wait for a few seconds before spawning again
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isIdle) return; // Prevent interaction if idle

        if (collision.GetContact(0).collider.name == "Shield")
        {
            isCollidingWithPlayer = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
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
        StopAllCoroutines();

        // get rid of enemy health bar
        Destroy(healthBar);

        // Change the enemy's color to pink
        GetComponent<EnemyFlash>().StopCoroutine(GetComponent<EnemyFlash>().flashCoroutine); //If enemy is flashing, stop flash
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(.97f, 0.51f, 0.48f, 1f); // Using magenta as a stand-in for really pink
        }

        // also set every other enemy to idle
        GameObject[] other_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in other_enemies)
        {
            var e = enemy.GetComponent<Long_Enemy>();
            if (e)
            {
                e.SetToIdle();
            }
            
        }

    }
}
