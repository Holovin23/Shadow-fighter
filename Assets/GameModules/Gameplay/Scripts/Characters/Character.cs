using System;
using Cysharp.Threading.Tasks.Triggers;
using GameModules.Gameplay.Scripts.Characters;
using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Health;
using GameModules.Gameplay.Scripts.Characters.Stats;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class Character : MonoBehaviour, IDamageable, IHealthOwner, IStatsOwner
{
    [SerializeField] protected Health _health;
    [SerializeField] private StatsConfig _stats;
    [SerializeField] private AttackController _attackController;
    
    protected IAnimatorProvider _animator;
    public event Action<IDamageable> OnDeath;
    public event Action<IDamageable, IDamageData> OnHit;
    public Transform Owner => transform;
    public bool IsDead { get; private set; }
    public IStatsProvider StatsProvider { get; private set; }
    public IHealthProvider HealthProvider => _health;
    public IAnimatorProvider AnimatorProvider => _animator;
    
    private void Start()
    {
        Init();
    }
    
    protected virtual void Init()
    {
        StatsProvider = _stats.CreateCopy();
        _health.Initialize(StatsProvider.GetStat(StatType.Health));
        _animator = GetComponentInChildren<IAnimatorProvider>();
        _attackController.Initialize(this);
        HealthProvider.OnDeathValue += OnDeathValue;
    }

    protected virtual void OnDeathValue(IHealthProvider value)
    {
        OnDeath?.Invoke(this);
    }
    
    public virtual void HandleDamage(IDamageData damageData)
    {
        HealthProvider.Remove((int)damageData.GetDamageValue());
        OnHit?.Invoke(this, damageData);
    }

    protected virtual void Die()
    {
        IsDead = true;
    }
}
