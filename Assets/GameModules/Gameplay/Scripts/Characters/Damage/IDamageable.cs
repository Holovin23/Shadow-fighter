using System;
using GameModules.Gameplay.Scripts.Characters.Health;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Damage
{
    public interface IDamageable
    {
        public event Action<IDamageable> OnDeath;
        public event Action<IDamageable, IDamageData> OnHit;

        public Transform Owner { get; }
        public bool IsDead { get; }
        public void HandleDamage(Damageable damageable, IDamageData damageData);
    }
}