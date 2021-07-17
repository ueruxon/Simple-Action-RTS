using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Normal", menuName = "Units State/Normal")]
public class NormalUnitState : UnitState_Base
{
    public override void Init() {
        
    }

    public override void UpdateState() {
        if (!BaseUnit.IsStopped()) {
            if (BaseUnit.GetNavMeshAgent().remainingDistance <= .5f) {
                BaseUnit.StopMoving();
                // ??
                StateIsFinished = true;
            }
        }
    }

    public void MoveTo(Vector3 destinationPosition) {
        BaseUnit.SetDestination(destinationPosition);
    }
}
