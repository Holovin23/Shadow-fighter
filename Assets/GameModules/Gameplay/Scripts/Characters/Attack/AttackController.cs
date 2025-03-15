using System;
using GameModules.Gameplay.Scripts.Characters.Stats;
using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class AttackController : MonoBehaviour
    {
        [Inject] protected IColliderService _colliderService;
        private float _attacksPerSecond;
        public event Action<AttackController> OnAttackStart;
        public event Action<AttackController> OnAttack;
        public event Action<AttackController> OnAttackEnd;
        
        protected Character _ownerCharacter;
        protected float _attackDelay => GetAttackRecoveryTime(_attacksPerSecond);
        public virtual void Initialize(Character owner)
        {
            _ownerCharacter = owner;
            ApplyStats(owner.StatsProvider);
        }

        private void ApplyStats(IStatsProvider statsProvider)
        {
            if (statsProvider.TryGetStat(StatType.AttacksPerSecond, out var attackPerSecondStat))
            {
                SetAttackSpeedValue(attackPerSecondStat.Value);
                attackPerSecondStat.OnValueChange += AttackPerSecondStat_OnValueChange;
            }
        }

        private void SetAttackSpeedValue(float value) => 
            _attacksPerSecond = value;
        
        private void AttackPerSecondStat_OnValueChange(Stat stat, float value) => 
            SetAttackSpeedValue(value);
        public void InvokeOnAttackStart() =>
            OnAttackStart?.Invoke(this);
        
        public void InvokeOnAttack() =>
            OnAttack?.Invoke(this);    
        
        public void InvokeOnAttackEnd() =>
            OnAttackEnd?.Invoke(this);

        public virtual void SetupProjectilesSpawner(ProjectilesSpawnHelper projectilesSpawnHelper) { }
        
        public static float GetAttackRecoveryTime(float attacksPerSecond) =>
            1f / attacksPerSecond;
    }
}