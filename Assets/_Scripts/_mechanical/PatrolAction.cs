using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/Actions/Patrol")]
public class PatrolAction : Action{
    public override void Act(BombCopNPCController controller){
        Patrol(controller);
    }
    private void Patrol(BombCopNPCController controller){
        controller.agent.destination = controller.waypointList[controller.nextWaypoint].position;
        controller.agent.isStopped = false;

        if(controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending){
            controller.nextWaypoint = (controller.nextWaypoint + 1) % controller.waypointList.Count;
        }
    }
}





