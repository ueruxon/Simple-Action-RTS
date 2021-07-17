using UnityEngine;

// Может получить урон
public interface IUnitDamageable
{
    bool IsDead();
    Vector3 GetPosition();
    void Damage(int damageAmount);
    float GetAttackDistanceOffset();
}
