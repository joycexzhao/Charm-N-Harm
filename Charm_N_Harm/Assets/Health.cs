//Test
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    [SerializeField] 
    private Slider slider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {   
        slider.value = currentValue / maxValue;
    }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead) // Check if the enemy is already dead
        {
            return; // If so, don't proceed further.
        }

        if (sender.layer == gameObject.layer) // Additional check to ignore hits from the same layer (e.g., enemies not hitting each other)
        {
            return;
        }

        currentHealth -= amount;
        UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            if (!isDead) // Check again to be extra cautious, though it's technically redundant due to the early return at the beginning
            {
                isDead = true; // Mark as dead to prevent further actions after death

                var enemyComponent = GetComponent<Enemy>();
                var longEnemyComponent = GetComponent<Long_Enemy>();
                var boss = GetComponent<Boss>();
                if (enemyComponent != null)
                {
                    enemyComponent.SetToIdle(); // Or any method that indicates death
                    EnemyManager.Instance.EnemyKilled();
                }
                else if (longEnemyComponent)
                {
                    longEnemyComponent.SetToIdle(); // Or any method that indicates death
                    EnemyManager.Instance.EnemyKilled();

                }
                else if (boss)
                {
                    boss.SetToIdle();
                    EnemyManager.Instance.EnemyKilled();
                }
            }
        }
    }

}
