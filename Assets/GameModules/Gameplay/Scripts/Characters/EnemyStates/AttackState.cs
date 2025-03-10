using UnityEngine;

public class AttackState : EnemyState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public AttackState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
        enemy.Animator?.SetSpeed(0f); // Останавливаем анимацию бега
        enemy.Animator?.StartAttack(); // ✅ Запускаем атаку
    }

    public override void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.ColliderHolder.Player.transform.position);
        
        if (distance > enemy.AttackRange)
        {
            stateMachine.ChangeState(EnemyStateType.Chase);
        }
        
        enemy.transform.LookAt(enemy.ColliderHolder.Player.transform.position);
        
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            enemy.Attack();
        }
    }

    public override void Exit()
    {
        enemy.Agent.isStopped = false;
    }
}