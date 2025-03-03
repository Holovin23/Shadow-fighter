using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Enemy : Character
{
    [Inject] private IColliderService _colliderHolder;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public float AttackRange = 2f;
    public float StunTime = 3f;

    public IAnimatorProvider Animator => _animator;
    public IColliderService ColliderHolder => _colliderHolder;
    public NavMeshAgent Agent => _navMeshAgent;

    private EnemyStateMachine stateMachine;

    private void Awake()
    {
        stateMachine = new EnemyStateMachine(this);
        DOVirtual.DelayedCall(7f, () =>
        {
            Debug.Log("Spawn");
            Spawn();
        });
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ResetEnemy()
    {
        //transform.position = new Vector3(0, 0, 0);
        Agent.enabled = true;
    }

    public void Attack()
    {
        Animator?.StartAttack();
        Debug.Log("Враг атакует игрока!");
    }

    public void Die()
    {
        Agent.enabled = false;
        Debug.Log("Враг умер!");
    }

    public void Spawn()
    {
        stateMachine.ChangeState(EnemyStateType.Initialize);
    }

    public void Stun()
    {
        stateMachine.ChangeState(EnemyStateType.Stun);
    }
}
