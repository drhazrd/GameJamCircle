using System;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/Decisions/Active")]
public class ActiveDecision: Decision
{
    public override bool Decide(BombCopNPCController controller){
        bool chaseTargetIsActive = controller.chaseTarget.gameObject.activeSelf;
        return chaseTargetIsActive;
    }
}
