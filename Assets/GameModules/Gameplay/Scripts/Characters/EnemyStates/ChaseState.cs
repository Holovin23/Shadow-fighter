using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private NavMeshAgent agent;
    private Character target;

    public ChaseState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        agent = enemy.Agent;
        target = enemy.ColliderHolder.Player;
        agent.isStopped = false;
    }

    public override void Update()
    {
        if (enemy.ColliderHolder == null) return;

        Vector3 playerPosition = enemy.ColliderHolder.Player.transform.position;
        enemy.Agent.SetDestination(playerPosition);

        float speed = enemy.Agent.velocity.magnitude / enemy.Agent.speed; // Значение от 0 до 1
        enemy.Animator?.SetSpeed(speed); // ✅ Передаем скорость в аниматор

        if (Vector3.Distance(enemy.transform.position, playerPosition) < enemy.AttackRange)
        {
            stateMachine.ChangeState(EnemyStateType.Attack);
        }
    }

    public override void Exit()
    {
        agent.isStopped = true;
    }
}