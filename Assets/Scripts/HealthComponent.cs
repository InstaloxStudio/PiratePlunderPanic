using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    //event for when the health changes
    public delegate void HealthChangedDelegate(float currentHealth, float maxHealth);
    public event HealthChangedDelegate OnHealthChanged;

    //event for when the health reaches 0
    public delegate void HealthReachedZeroDelegate();
    public event HealthReachedZeroDelegate OnHealthReachedZero;


    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0f)
        {
            return;
        }


        currentHealth -= damage;
        if (OnHealthChanged != null)
        {
            OnHealthChanged(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            if (OnHealthReachedZero != null)
            {
                OnHealthReachedZero();
            }
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Die()
    {
        
    }


}