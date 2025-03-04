public class PoolState : EnemyState
{
    public PoolState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        enemy.gameObject.SetActive(false);
    }

    public override void Update() { }

    public override void Exit()
    {
        enemy.gameObject.SetActive(true);
    }
}