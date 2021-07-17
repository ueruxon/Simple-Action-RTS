using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitVisual : MonoBehaviour
{
    [Header("Визуал")]
    [Space(2)]
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _walkingParticle;
    [SerializeField] private ParticleSystem _landingParticle;
    [SerializeField] private GameObject _selectedVisualObject;

    private Unit _unit;

    private void Awake() {
        _unit = GetComponent<Unit>();
    }

    private void Start() {
        UpdateSelected();

        _unit.OnSelectedChanged += Unit_OnSelectedChanged;
        _unit.OnStartedMoving += Unit_OnStartedMoving;
        _unit.OnStoppedMoving += Unit_OnStoppedMoving;
    }

    #region Майнинг
    public void MinedResource() {
        _landingParticle.Play();
    }

    public void StopMining() {
        _animator.SetTrigger("Attack");
    }
    #endregion 

    public void Attack() {
        print("atacck");
        // партиклы
        _animator.SetTrigger("Attack");
    }

    public void SetIdle() {
        _animator.SetBool("IsWalking", false);
    }

    private void OnDisable() {
        _unit.OnSelectedChanged -= Unit_OnSelectedChanged;
        _unit.OnStartedMoving -= Unit_OnStartedMoving;
        _unit.OnStoppedMoving -= Unit_OnStoppedMoving;
    }

    private void Unit_OnStoppedMoving() {
        _walkingParticle.Stop();
        _animator.SetBool("IsWalking", false);
        //_animator.SetBool("IsAttacking", false);
    }

    private void Unit_OnStartedMoving(bool isEnemy = false) {
        if (isEnemy == false)
            _walkingParticle.Play();
        // _animator.SetBool("IsAttacking", false);
        _animator.SetBool("IsWalking", true);
        _animator.SetFloat("MovingSpeed", 1f);  
    }

    private void UpdateSelected() {
        _selectedVisualObject.SetActive(_unit.GetIsSelected());
    }

    private void Unit_OnSelectedChanged() {
        UpdateSelected();
    }
}
