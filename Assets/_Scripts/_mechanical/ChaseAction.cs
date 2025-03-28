using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/Actions/Chase")]
public class ChaseAction : Action
{
    // Start is called before the first frame update
    public override void Act(BombCopNPCController controller)
    {
        Chase(controller);
    }

    // Update is called once per frame
    void Chase(BombCopNPCController controller)
    {
        controller.agent.destination = controller.chaseTarget.position;
        controller.agent.isStopped =  false;
    }
}
