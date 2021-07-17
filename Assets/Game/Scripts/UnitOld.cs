using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitOld : MonoBehaviour
{
    public enum State {
        Normal,
        Gathering_GointTo,
        Gathering_Active,
        Gathering_GointBack
    }

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _selectedVisualObject;
    [Header("Настройки юнита")]
    [Space(1)]
    [SerializeField] private bool isEnemy;
    // время на добычу одной еденицы ресусра
    [SerializeField] private float _resourceGatherTimerMax = 1.5f;
    // сколько вмещает в себя единиц ресурсов юнит
    [SerializeField] private int _maxResourceCapacity = 5;

    private NavMeshAgent _meshAgent;
    private ResourceNode _resourceNode;

    private State _state;
    private bool isSelected;
    private float _resourceGatherTimer;
    private int _resourceAmount;
    private float _reachedDistance = 1f;


    private void Awake() {
        _meshAgent = GetComponent<NavMeshAgent>();

        _state = State.Normal;
        SetSelected(false);
    }

    private void Update() {
        switch (_state) {
            case State.Normal:
                break;
            case State.Gathering_GointTo:
                SetDestination(_resourceNode.GetPosition());

                if (Vector3.Distance(transform.position, _resourceNode.GetPosition()) < _reachedDistance) {
                    _state = State.Gathering_Active;
                    _resourceGatherTimer += _resourceGatherTimerMax;
                }

                break;
            case State.Gathering_Active:
                _resourceGatherTimer -= Time.deltaTime;

                if (_resourceGatherTimer < 0f) {
                    _resourceAmount++;
                    _resourceGatherTimer += _resourceGatherTimerMax;

                    if (_resourceAmount >= _maxResourceCapacity ) {
                        _state = State.Gathering_GointBack;
                    }
                }

                break;
            case State.Gathering_GointBack:
                // возвращаемся на ближайший склад чтобы положить ресурсы
                SetDestination(Vector3.zero);

                if (Vector3.Distance(transform.position, Vector3.zero) < _reachedDistance) {
                    ResourceManager.Instance.AddResourceAmount(_resourceNode.GetResourceTypeSO(), _resourceAmount);
                    _resourceAmount = 0;

                    // идем обратно добывать ресурс если нода не выработана
                    _state = State.Gathering_GointTo;
                }

                break;
        }
    }


    private void SetDestination(Vector3 destinationPosition) {
        _meshAgent.SetDestination(destinationPosition);
    }

    public void MoveTo(Vector3 destinationPosition) {
        _state = State.Normal;

        SetDestination(destinationPosition);
    }

    public void SetSelected(bool selected) {
        isSelected = selected;

        _selectedVisualObject.SetActive(isSelected);
    }

    public void SetResourceNode(ResourceNode node) {
        _resourceNode = node;

        _state = State.Gathering_GointTo;
    }

    public bool IsEnemy() {
        return isEnemy;
    }
}
