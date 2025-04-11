using UnityEngine;
[CreateAssetMenu(menuName ="AI/Actions/Attack")]

public class AttackAction : ActionBase
{
    public override void Act(BombCopNPCController controller)
    {
        Attack(controller);

    }

    private void Attack(BombCopNPCController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.lookRange, Color.red);

        if(Physics.SphereCast(controller.eyes.position, controller.lookRadius, controller.eyes.forward, out hit, controller.attackRange)&& hit.collider.CompareTag("Player")){
                controller.Attack(controller.attackForce, controller.attackRate);
        }
    }
}
