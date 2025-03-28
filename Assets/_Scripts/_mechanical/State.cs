using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/State")]
public class State: ScriptableObject{
    public Action[] actions;
    public Trasistion[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public Color gizmoColor = Color.green;

    public void UpdateState(BombCopNPCController copNPCController){
        DoActions(copNPCController);
        CheckTransition(copNPCController);
    }
    private void DoActions(BombCopNPCController copNPCController){
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(copNPCController);
        }
    }

    private void CheckTransition(BombCopNPCController controller){
        for(int i = 0; i < transitions.Length; i++){
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if(decisionSucceeded){
                controller.TransitionToState(transitions[i].trueState);
            } else {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}