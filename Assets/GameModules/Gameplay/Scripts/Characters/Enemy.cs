using System;
using DG.Tweening;
using GameModules.Gameplay.Scripts.Characters.Damage;
using TFPlay.Modules.Core.TickService;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Enemy : Character , IDamageable
{
    [Inject] private IColliderService _colliderHolder;
    [Inject] private SignalBus _signalBus;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Collider _mainCollider;
    public float AttackRange = 2f;
    public float StunTime = 3f;
    
    public IAnimatorProvider Animator => _animator;
    public IColliderService ColliderHolder => _colliderHolder;
    public NavMeshAgent Agent => _navMeshAgent;

    private EnemyStateMachine stateMachine;

    private void Awake()
    {
        _signalBus.Subscribe<TickSignal>(UpdateByTick);
        stateMachine = new EnemyStateMachine(this);
        DOVirtual.DelayedCall(7f, () =>
        {
            Debug.Log("Spawn");
            Spawn();
        });
        _colliderHolder.AddCharacter(_mainCollider, this);
    }

    private void UpdateByTick(TickSignal tickSignal)
    {
        stateMachine.Update();
    }
    /*private void Update()
    {
        stateMachine.Update();
    }*/

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
        _signalBus.Unsubscribe<TickSignal>(UpdateByTick);
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
