using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    public UnityAction<float> onDamageTaken;
    public UnityAction onDeath;

    public void Set(float health)
    {
        _currentHealth = (this._maxHealth = health);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        onDamageTaken?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

    private void Die()
    {
        onDeath?.Invoke();
    }

    public float GetHealth() => _currentHealth;

    public float GetMaxHealth() => _maxHealth;
}
