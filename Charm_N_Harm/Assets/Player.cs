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
      
    }

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        pointerInput = GetPointerInput();

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
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
       
    }

}
