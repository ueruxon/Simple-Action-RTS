using UnityEngine;
using UnityEngine.Events;
using CodeMonkey.Utils;

public class Building : MonoBehaviour, IUnitDamageable
{
    public event UnityAction OnDead;

    [SerializeField] private int _healthAmountMax;
    [SerializeField] private bool _isEnemy;
    [SerializeField] private float _attackDistanceOffset;
    [SerializeField] private float _healthBarOffsetY;

    private HealthSystem _healthSystem;
    private World_Bar _healthBar;

    private void Awake() {
        _healthSystem = new HealthSystem(_healthAmountMax);

        _healthBar = World_Bar.Create(transform, new Vector3(0, _healthBarOffsetY, 0), new Vector3(2, .3f), Color.grey, Color.red, 1f, 0, new World_Bar.Outline { color = Color.black, size = .1f });
        LookAtCamera lookAtCamera = _healthBar.GetGameObject().AddComponent<LookAtCamera>();
        lookAtCamera.SetInvert(true);

        _healthBar.Hide();

        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead() {
        Destroy(gameObject);
        OnDead?.Invoke();
    }

    private void HealthSystem_OnHealthChanged() {
        if (_healthSystem.GetHealthNormalized() < 1f) {
            _healthBar.Show();
            _healthBar.SetSize(_healthSystem.GetHealthNormalized());
        }
        else {
            _healthBar.Hide();
        }
    }

    public bool IsEnemy() {
        return _isEnemy;
    }

    public bool IsDead() {
        return _healthSystem.IsDead();
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Damage(int damageAmount) {
        _healthSystem.Damage(damageAmount);
    }

    public Transform GetTransform() {
        return transform;
    }

    public float GetAttackDistanceOffset() {
        return _attackDistanceOffset;
    }
}
