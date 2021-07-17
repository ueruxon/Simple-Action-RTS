using UnityEngine;

[CreateAssetMenu(fileName = "Mining", menuName = "Units State/Mining")]
public class MiningUnitState : UnitState_Base
{
    // время на добычу одной еденицы ресусра
    [SerializeField] private float _resourceMiningTimerMax = 2f;

    private enum State {
        GoingToResourceNode,
        Mining,
        GoingToStorage,
    }

    private State _currentState;
    private ResourceNode _resourceNode;
    private ResourceTypeSO _lastResourceNodeResourceType;
    private Vector3 _lastResourceNodePosition;
    private Storage _storage;
    private UnitVisual _unitVisualObject;

    private float _reachedDistance = 2.5f;
    private float _resourceMiningTimer;

    public override void Init() {
        _unitVisualObject = BaseUnit.GetUnitVisualObject();
    }

    public void SetResourceNode(ResourceNode resourceNode) {
        _resourceNode = resourceNode;
        _lastResourceNodePosition = resourceNode.GetPosition();
        _lastResourceNodeResourceType = resourceNode.GetResourceTypeSO();

        _currentState = State.GoingToResourceNode;
    }

    public override void UpdateState() {
        switch (_currentState) {
            case State.GoingToResourceNode:
                if (TryToFinishCurrentMining()) break;

                if (_resourceNode != null) {
                    BaseUnit.SetDestination(_resourceNode.GetPosition());

                    if (Vector3.Distance(BaseUnit.GetPosition(), _resourceNode.GetPosition()) < _reachedDistance) {
                        // Мы дошли до ноды
                        BaseUnit.StopMoving();
                        // смотрим на ноду
                        BaseUnit.transform.LookAt(_resourceNode.GetPosition());
                        // Начинаем добывать
                        _currentState = State.Mining;
                    }
                }

                break;
            case State.Mining:
                if (TryToFinishCurrentMining()) break;
                _resourceMiningTimer -= Time.deltaTime;

                if (_resourceMiningTimer < 0f) {
                    _resourceMiningTimer = _resourceMiningTimerMax;

                    // анимация
                    _unitVisualObject.Attack();

                    // добавим ресурс в инвентарь если есть место
                    bool resourceAdded = BaseUnit.TryAddResourceToInventary(_resourceNode.GetResourceTypeSO());

                    if (resourceAdded) {
                        // отнимаем от ноды
                        _resourceNode.GrabResource();
                        // анимация
                        _unitVisualObject.MinedResource();

                    } else {
                        _currentState = State.GoingToStorage;

                        // анимация
                        _unitVisualObject.StopMining();
                    }
                }
                break;
            case State.GoingToStorage:
                Vector3 storagePosition = _storage.GetPosition();

                float reachedDistance = 3f;
                BaseUnit.SetDestination(storagePosition, reachedDistance - .5f);

                if (Vector3.Distance(BaseUnit.GetPosition(), storagePosition) < reachedDistance) {
                    BaseUnit.StopMoving();
                    // Ресурсы на склад
                    BaseUnit.ResourceToStorage();

                    _currentState = State.GoingToResourceNode;
                }
                break;
        }
    }

    private bool TryGoToStorage() {
        // если инвентарь заполнен
        if (BaseUnit.InventoryIsFull()) {
            // идем к ближайшему складу
            _storage = Storage.GetClosestStorage(BaseUnit.GetPosition());
            if (_storage != null) {
                _currentState = State.GoingToStorage;
                return true;
            }
            else
                // Склада еще нет
                return false;
        }
        
         // Если место в инвентаре есть, можем работать дальше
         return false;
    }

    private bool IsResourceNodeWorkedOut() {
        return _resourceNode == null || !_resourceNode.HasResources();
    }

    private bool TryToFinishCurrentMining() {
        // если ресурсная нода выработана
        if (IsResourceNodeWorkedOut()) {
            Debug.Log("tut + " + BaseUnit.name);
            if (TryGoToStorage()) {
                // Направляемся к хранилищу
                return true;
            } else {
                // Если ресурсная нода выработана, но у нас есть место в инвентаре, то можем попытаться найти другую рядом
                if (TryFindClosestResourceNode()) {
                    // Found new resource node
                    return true;
                }
                else {
                    // Рядом нет ничего полезного (стоим и ждем приказа)
                    BaseUnit.NormalMoveTo(BaseUnit.GetPosition());

                    // анимация
                    _unitVisualObject.SetIdle();
                    return true;
                }
            }
        } else {
            // У ноды есть ресурсы, но инвентарь заполнен?
            if (BaseUnit.InventoryIsFull()) {
                // Пробуем пойти к хранилищу
                if (TryGoToStorage()) {
                    return true;
                }
                else {
                    // Рядом хранилища нет, ждем приказа
                    BaseUnit.NormalMoveTo(BaseUnit.GetPosition());
                    // анимация
                    _unitVisualObject.SetIdle();
                    return true;
                }
            }
            else {
                // Рядом есть "хорошая" нода и инвентарь не полный
                return false;
            }
        }
    }

    private bool TryFindClosestResourceNode() {
        if (_lastResourceNodeResourceType != null) {
            float findResourceNodeRange = 10f;
            Collider[] colliderArray = Physics.OverlapSphere(_lastResourceNodePosition, findResourceNodeRange);

            ResourceNode closestResourceNode = null;
            foreach (Collider collider in colliderArray) {
                if (collider.TryGetComponent(out ResourceNode newResourceNode)) {
                    if (!newResourceNode.HasResources()) continue;
                    // Если ресурсная нода имеет другой тип, то пропускаем ее
                    if (newResourceNode.GetResourceTypeSO() != _lastResourceNodeResourceType) continue; 

                    if (closestResourceNode == null) {
                        closestResourceNode = newResourceNode;
                    }
                    else {
                        if (Vector3.Distance(newResourceNode.GetPosition(), _lastResourceNodePosition) <
                            Vector3.Distance(closestResourceNode.GetPosition(), _lastResourceNodePosition)) {
                            // Эта ближайшая
                            closestResourceNode = newResourceNode;
                        }
                    }
                }
            }

            if (closestResourceNode != null) {
                SetResourceNode(closestResourceNode);
                return true;
            }
        }

        return false;
    }
}
