public abstract class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public enum EnemyStateType
{
    Pool,
    Initialize,
    Chase,
    Attack,
    Death,
    Stun
}