using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    public float flashDuration = 0.5f; // Duration of the flash effect
    private Image flashImage; // Reference to the UI Image
    private Color flashColor = new Color(1, 0, 0, 0.5f); // Red color with half transparency
    private Color clearColor = new Color(1, 0, 0, 0); // Red color with full transparency
    public GameObject hitSound1;
    public GameObject hitSound2;
    //private GameObject[] hitSounds = { hitSound1, hitSound2 };

    void Start()
    {
        
        flashImage = GetComponent<Image>();
        flashImage.color = clearColor; // Ensure the flash image is transparent at start
    }

    public void Flash()
    {
        StopAllCoroutines(); // Stop previous flashes if any
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        //GameObject[] hitSounds = { hitSound1, hitSound2 };
        float elapsedTime = 0;

        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            hitSound1.GetComponent<AudioSource>().Play();
        }
        else
        {
            hitSound2.GetComponent<AudioSource>().Play();
        }
        //hitSounds[rand].GetComponent<AudioSource>().Play();

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            flashImage.color = Color.Lerp(flashColor, clearColor, elapsedTime / flashDuration);
            yield return null;
        }

        flashImage.color = clearColor; // Ensure it's fully transparent after the flash
    }
}
