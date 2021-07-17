using UnityEngine;

public abstract class UnitState_Base : ScriptableObject
{
    public bool StateIsFinished { get; protected set; }
    [HideInInspector] public Unit BaseUnit;

    public virtual void Init() { }

    public abstract void UpdateState();
}
