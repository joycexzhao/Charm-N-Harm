using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float health = 100f;
    public TextMeshProUGUI healthDisplay;
    public DamageFlash damageFlash;
    public UnityEngine.UI.Slider healthBarSlider;
    private Rigidbody2D rb;
    private Vector2 pointerInput;
    private Vector2 move;
    private Coroutine damageCoroutine;
    private UnityEngine.UI.Image healthFillImage;
    private WeaponParent weaponParent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParent>();

        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
        healthFillImage = healthBarSlider.fillRect.GetComponent<UnityEngine.UI.Image>();
        UpdateHealthDisplay();
        UpdateHealthBarColor();
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
        if (collision.gameObject.CompareTag("Enemy") && !collision.gameObject.GetComponent<Enemy>().IsIdle)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageOverTime());
            }
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
            health = Mathf.Clamp(health - 10f, 0, healthBarSlider.maxValue);
            healthBarSlider.value = health;
            UpdateHealthBarColor();
            UpdateHealthDisplay();
            damageFlash.Flash();

            if (health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void UpdateHealthDisplay()
    {
        healthDisplay.text = "Health: " + Mathf.RoundToInt(health).ToString();
    }

    void UpdateHealthBarColor()
    {
        float healthPercent = health / healthBarSlider.maxValue;

        if (healthPercent > 0.7f)
        {
            healthFillImage.color = Color.green;
        }
        else if (healthPercent > 0.3f)
        {
            healthFillImage.color = Color.yellow;
        }
        else
        {
            healthFillImage.color = Color.red;
        }
    }
}
