using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using CodeMonkey.Utils;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour, IUnitDamageable
{
    public event UnityAction OnSelectedChanged;
    public event UnityAction<bool> OnStartedMoving;
    public event UnityAction OnStoppedMoving;
    public event UnityAction<Unit> OnDead;

    [Header("Настройки юнита")]
    [Space(1)]
    [SerializeField] private bool _isEnemy;
    [SerializeField] private int _healthAmountMax = 30;
    // сколько вмещает в себя единиц ресурсов юнит
    [SerializeField] private int _maxResourceCapacity = 5;
    [SerializeField] private float _attackDistanceOffset = 2f;

    [Space(2)]
    [SerializeField] private UnitVisual _unitVisual;

    [Header("Возможные состояния юнита")]
    [Space(1)]
    [SerializeField] private UnitState_Base _normalUnitState;
    [SerializeField] private UnitState_Base _miningUnitState;
    [SerializeField] private UnitState_Base _constructionUnitState;
    [SerializeField] private UnitState_Base _patrolUnitState;
    [SerializeField] private UnitState_Base _attackUnitState;

    [Header("Текущее состояние юнита")]
    [Space(2)]
    public UnitState_Base CurrentState;
    public bool IsInAttack = false;

    private NavMeshAgent _meshAgent;
    private HealthSystem _healthSystem;
    private World_Bar _healthBar;
    private bool _isSelected;

    private List<ResourceTypeSO> _inventoryResourceTypeList;

    private void Awake() {
        _meshAgent = GetComponent<NavMeshAgent>();

        _healthSystem = new HealthSystem(_healthAmountMax);
        _inventoryResourceTypeList = new List<ResourceTypeSO>();


        _healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;

        MakeWorldHealthBar();
        SetStateByDefualt();
        SetSelected(false);
    }

    private void Update() {
        if (CurrentState.StateIsFinished == false)
            CurrentState.UpdateState();
    }

    private void OnDisable() {
        _healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        _healthSystem.OnDead -= HealthSystem_OnDead;
    }

    private void MakeWorldHealthBar() {
        _healthBar = World_Bar.Create(transform, new Vector3(0, 2, 0), new Vector3(1, .2f), Color.grey, Color.red, 1f, 0, new World_Bar.Outline { color = Color.black, size = .1f });

        LookAtCamera lookAtCamera = _healthBar.GetGameObject().AddComponent<LookAtCamera>();
        lookAtCamera.SetInvert(true);
        lookAtCamera.SetZeroY(true);
    }

    // установка нового состояния
    private void SetState(UnitState_Base state) {
        CurrentState = Instantiate(state);
        CurrentState.BaseUnit = this;
        CurrentState.Init();

        IsInAttack = false;
    }

    public void SetSelected(bool selected) {
        _isSelected = selected;
        OnSelectedChanged?.Invoke();
    }

    public bool GetIsSelected() => _isSelected;

    public void SetStateByDefualt() {
        if (_isEnemy) {
            SetState(_patrolUnitState);

            OnStartedMoving?.Invoke(_isEnemy);
        }
        else {
            SetState(_normalUnitState);

            OnStoppedMoving?.Invoke();
        }
    }

    #region Движение
    public void SetDestination(Vector3 destinationPosition, float stoppingDistance = .5f) {
        _meshAgent.SetDestination(destinationPosition);
        _meshAgent.stoppingDistance = stoppingDistance;
        _meshAgent.isStopped = false;

        OnStartedMoving?.Invoke(_isEnemy);
    }

    public void NormalMoveTo(Vector3 destinationPosition) {
        SetState(_normalUnitState);

        NormalUnitState normalUnitState = (NormalUnitState)CurrentState;
        normalUnitState.MoveTo(destinationPosition);
    }

    public void StopMoving() {
        _meshAgent.isStopped = true;

        OnStoppedMoving?.Invoke();
    }

    public bool IsStopped() {
        return _meshAgent.isStopped;
    }
    #endregion

    #region Майнинг
    public void SetMiningResource(ResourceNode node) {
        if (_miningUnitState == null) {
            SetStateByDefualt();
        } else {
            SetState(_miningUnitState);
        }
        
        MiningUnitState miningUnitState = (MiningUnitState)CurrentState;
        miningUnitState.SetResourceNode(node);
    }

    public bool TryAddResourceToInventary(ResourceTypeSO resourceType) {
        if (_inventoryResourceTypeList.Count < _maxResourceCapacity) {
            _inventoryResourceTypeList.Add(resourceType);
            return true;
        }

        return false;
    }

    public bool InventoryIsFull() => _inventoryResourceTypeList.Count >= _maxResourceCapacity;

    public void ResourceToStorage() {
        // добавляем ресурсы на склад
        ResourceManager.Instance.AddResourceAmount(_inventoryResourceTypeList);
        _inventoryResourceTypeList.Clear();
    }
    #endregion

    #region Атака
    public void SetTarget(IUnitDamageable targetUnit) {
        if (_attackUnitState == null) {
            SetStateByDefualt();
        } else {
            SetState(_attackUnitState);
        }

        AttackUnitState attackUnitState = (AttackUnitState)CurrentState;
        attackUnitState.SetEnemyTarget(targetUnit);
        IsInAttack = true;
    }
    #endregion

    #region Строительство
    public void SetConstructionBuilding(BuildingConstruction buildingConstruction) {
        SetState(_constructionUnitState);

        ConstructionUnitState constructionUnitState = (ConstructionUnitState)CurrentState;
        constructionUnitState.SetBuildingConstruction(buildingConstruction);
    }

    #endregion

    #region Ивенты и события
    private void HealthSystem_OnDead() {

        OnDead?.Invoke(this);
        Destroy(gameObject);
    }

    private void HealthSystem_OnHealthChanged() {
        _healthBar.SetSize(_healthSystem.GetHealthNormalized());
    }
    #endregion

    public NavMeshAgent GetNavMeshAgent() {
        return _meshAgent;
    }

    public UnitVisual GetUnitVisualObject() {
        return _unitVisual;
    }

    #region Методы интерфейса
    public bool IsEnemy() {
        return _isEnemy;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public bool IsDead() {
        return _healthSystem.IsDead();
    }

    public void Damage(int damageAmount) {
        print("урон");
        if (IsDead()) return; // Already dead

        _healthSystem.Damage(damageAmount);
    }

    public float GetAttackDistanceOffset() {
        return _attackDistanceOffset;
    }
    #endregion
}
