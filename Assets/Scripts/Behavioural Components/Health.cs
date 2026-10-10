using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public UnityEvent onZeroHealth;
    [SerializeField] Image healthBar;
    [SerializeField] float maxHealth;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void AddHealth(float amount)
    {
        if ((currentHealth + amount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
    }

    public void SubtractHealth(float amount)
    {
        if ((currentHealth - amount) < 0)
        {
            onZeroHealth.Invoke();
        }
        else
        {
            currentHealth -= amount;
        }
    }
}
