using UnityEngine;

public class BattlePlatforms : MonoBehaviour
{
    BattleFlowStateMachine _bfsm;
    MeshRenderer _meshRenderer;

    private void OnEnable()
    {
        _bfsm = ServiceProvider.Instance.ServiceLocator.Get<BattleFlowStateMachine>();

        _bfsm.BattleFlowStateMachineEventBus.OnPlayersExit += EnablePlatform;
        _bfsm.BattleFlowStateMachineEventBus.OnPlayersTurn += EnablePlatform;

        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnDisable()
    {
        _bfsm.BattleFlowStateMachineEventBus.OnPlayersExit -= EnablePlatform;
        _bfsm.BattleFlowStateMachineEventBus.OnPlayersTurn -= EnablePlatform;
    }

    private void EnablePlatform(BattleFlowStateMachineEventContext ctx)
    {
        _meshRenderer.enabled = !_meshRenderer.enabled;
    }
}
