
using System;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/Decisions/Scan")]
public class ScanDecision: Decision
{
    public override bool Decide(BombCopNPCController controller){
        bool noEnemyInSight = Scan(controller);
        return noEnemyInSight;
    }

    private bool Scan(BombCopNPCController controller)
    {
        controller.agent.isStopped = true;
        controller.transform.Rotate(0, controller.searchTurnSpeed * Time.deltaTime,0);
        return controller.CheckIfCountdownElapsed(controller.searchDuration);
    }
}
