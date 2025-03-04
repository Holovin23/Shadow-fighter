using System.Collections;
using UnityEngine;

public class DeathState : EnemyState
{
    public DeathState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        enemy.Die();
        enemy.StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(2f); // Задержка перед респавном
        stateMachine.ChangeState(EnemyStateType.Pool);
    }

    public override void Update() { }
    public override void Exit() { }
}