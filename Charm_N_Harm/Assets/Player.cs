using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Include this namespace for scene management

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float health = 100f;
    public TextMeshProUGUI healthDisplay;

    private Rigidbody2D rb;
    private Vector2 pointerInput;
    private Vector2 move;
    private Coroutine damageCoroutine;

    private WeaponParent weaponParent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateHealthDisplay();
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        pointerInput = GetPointerInput();
        weaponParent.PointerPosition = pointerInput;

        if (Input.GetMouseButtonDown(0))
        {
            weaponParent.Attack();
        }
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(DamageOverTime());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            health -= 10f;
            UpdateHealthDisplay();

            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
                yield break; // Exit the coroutine
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second before applying damage again
        }
    }

    void UpdateHealthDisplay()
    {
        healthDisplay.text = "Health: " + health;
    }
}
