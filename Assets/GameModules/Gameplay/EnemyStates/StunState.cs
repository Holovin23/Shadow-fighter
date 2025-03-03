using System.Collections;
using UnityEngine;

public class StunState : EnemyState
{
    private float stunDuration;
    private Coroutine stunCoroutine;

    public StunState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.SetStun(); // Если у тебя есть анимация оглушения

        stunDuration = enemy.StunTime;
        stunCoroutine = enemy.StartCoroutine(StunTimer());
    }

    private IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(stunDuration);
        stateMachine.ChangeState(EnemyStateType.Chase); // После оглушения враг продолжает преследование
    }

    public override void Update() { }

    public override void Exit()
    {
        if (stunCoroutine != null)
            enemy.StopCoroutine(stunCoroutine);

        enemy.Agent.isStopped = false;
    }
}