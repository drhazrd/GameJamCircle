using System;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/Decisions/Look")]
public class LookDecision: Decision
{
    public override bool Decide(BombCopNPCController controller){
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(BombCopNPCController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.lookRange, Color.green);

        if(Physics.SphereCast(controller.eyes.position, controller.lookRadius, controller.eyes.forward, out hit, controller.lookRange)&& hit.collider.CompareTag("Player")){
            controller.chaseTarget = hit.transform;
            return true;
        } else {
            return false;
        }
    }
}
