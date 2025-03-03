using System.Collections.Generic;

public class EnemyStateMachine
{
    private Dictionary<EnemyStateType, EnemyState> states;
    private EnemyState currentState;

    public EnemyStateMachine(Enemy enemy)
    {
        states = new Dictionary<EnemyStateType, EnemyState>
        {
            { EnemyStateType.Pool, new PoolState(this, enemy) },
            { EnemyStateType.Initialize, new InitializeState(this, enemy) },
            { EnemyStateType.Chase, new ChaseState(this, enemy) },
            { EnemyStateType.Attack, new AttackState(this, enemy) },
            { EnemyStateType.Death, new DeathState(this, enemy) },
            { EnemyStateType.Stun, new StunState(this, enemy) } // Добавили Stun
        };
    }

    public void ChangeState(EnemyStateType newState)
    {
        currentState?.Exit();
        currentState = states[newState];
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}