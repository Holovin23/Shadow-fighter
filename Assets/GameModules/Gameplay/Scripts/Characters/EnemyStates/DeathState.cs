using System.Collections;
using UnityEngine;

public class DeathState : EnemyState
{
    public DeathState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        enemy.Die();
        stateMachine.ChangeState(EnemyStateType.Pool);
       // enemy.StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(1f); // Задержка перед респавном
        
    }

    public override void Update() { }
    public override void Exit() { }
}