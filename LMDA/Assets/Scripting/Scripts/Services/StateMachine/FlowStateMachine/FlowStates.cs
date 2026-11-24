public class FlowStates
{
    public FlowState_Overworld FlowStateEnterOverWorld;
    public FlowState_Dialog FlowStateEnterDialog;
    public FlowState_Pause FlowStatePause;
    public FlowState_StatTutorial FlowStateStatTutorial;
    public FlowState_Cinematic FlowStateCinematic;

    public FlowStates(FlowStateMachine stateMachine)
    {
        FlowStateEnterOverWorld = new FlowState_Overworld(stateMachine);
        FlowStateEnterDialog = new FlowState_Dialog(stateMachine);
        FlowStatePause = new FlowState_Pause(stateMachine);
        FlowStateStatTutorial = new FlowState_StatTutorial(stateMachine);
        FlowStateCinematic = new FlowState_Cinematic(stateMachine);
    }
}