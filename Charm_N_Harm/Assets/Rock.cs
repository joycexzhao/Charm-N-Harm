using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb;
    public Transform playerTransform;
    public float rockSpeed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Invoke("Die", 0.2f);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        rb.velocity = (directionToPlayer * rockSpeed);
    }
}
