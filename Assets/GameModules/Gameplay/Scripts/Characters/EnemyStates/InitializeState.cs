public class InitializeState : EnemyState
{
    public InitializeState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        enemy.ResetEnemy();
        stateMachine.ChangeState(EnemyStateType.Chase);
    }

    public override void Update() { }
    public override void Exit() { }
}