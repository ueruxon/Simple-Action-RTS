using System;

public class HealthSystem {

    public event Action OnHealthChanged;
    public event Action OnHealthMaxChanged;
    public event Action OnDamaged;
    public event Action OnHealed;
    public event Action OnDead;

    private int healthMax;
    private int health;

    public HealthSystem(int healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth() {
        return health;
    }

    public int GetHealthMax() {
        return healthMax;
    }

    public float GetHealthNormalized() {
        return (float)health / healthMax;
    }

    public void Damage(int amount) {
        health -= amount;
        if (health < 0) {
            health = 0;
        }

        OnHealthChanged?.Invoke();
        OnDamaged?.Invoke();

        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        OnDead?.Invoke();
    }

    public bool IsDead() {
        return health <= 0;
    }

    public void Heal(int amount) {
        health += amount;
        if (health > healthMax) {
            health = healthMax;
        }
        OnHealthChanged?.Invoke();
        OnHealed?.Invoke();
    }

    public void HealComplete() {
        health = healthMax;
        OnHealthChanged?.Invoke();
        OnHealed?.Invoke();
    }

    public void SetHealthMax(int healthMax, bool fullHealth) {
        this.healthMax = healthMax;
        if (fullHealth) health = healthMax;
        OnHealthMaxChanged?.Invoke();
        OnHealthChanged?.Invoke();
    }

}
