using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
        {
            return;
        }
        if (sender.layer == gameObject.layer)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            if (!isDead)
            {
                //OnDeathWithReference?.Invoke(sender);
                //isDead = true;
                //// Notify the Enemy script instead of destroying the GameObject
                //GetComponent<Enemy>().SetToIdle();

                // Attempt to get the Enemy component
                var enemyComponent = GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    // This will now set the enemy to dead instead of idle
                    enemyComponent.SetToIdle(); // Assuming SetToDead() is a method you have or will implement
                }
                else
                {
                    // Attempt to get the Long_Enemy component
                    var longEnemyComponent = GetComponent<Long_Enemy>();
                    if (longEnemyComponent != null)
                    {
                        // Assuming Long_Enemy also has a SetToDead method or similar mechanism
                        longEnemyComponent.SetToIdle();
                    }
                }
            }
        }
    }
}
