using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

public class EnemyFlash : MonoBehaviour
{
    public float flashDuration = 0.5f; // Duration of the flash effect
    private Color flashColor = new Color(1, 0, 1, 1); // Magenta Color
    private SpriteRenderer spriteR;

    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }

    public void Flash()
    {
        StopAllCoroutines(); // Stop previous flashes if any
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        float elapsedTime = 0;

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            spriteR.color = Color.Lerp(flashColor, Color.white, elapsedTime / flashDuration);
            yield return null;
        }
    }
}
