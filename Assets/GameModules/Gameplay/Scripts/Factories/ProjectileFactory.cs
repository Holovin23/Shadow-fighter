using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameModules.Gameplay.Scripts.Characters;
using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Stats;
using GameModules.Gameplay.Scripts.Projectiles;
using Pooling;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Factories
{
    public class ProjectilesFactory
    {
        private Projectile projectilePrefab;
        private IStatsProvider statsProvider;
        private ProjectileAttributesSetupVisitor setupVisitor;
        private ProjectilesSpawnHelper spawnHelper;
        private List<IProjectile> projectiles = new List<IProjectile>();
        private IPoolService _poolService;

        public ProjectilesFactory(IPoolService poolService, IStatsProvider statsProvider, Projectile projectilePrefab, ProjectilesSpawnHelper spawnHelper)
        {
            this.spawnHelper = spawnHelper;
            this.projectilePrefab = projectilePrefab;
            this.statsProvider = statsProvider;
            _poolService = poolService;
            setupVisitor = new ProjectileAttributesSetupVisitor(statsProvider);
        }

        public void Create(IDamageData damageData, Action<List<IProjectile>> result)
        {
            projectiles = new List<IProjectile>();

            CoroutineRunner.StartRoutine(CreateRoutine());

            IEnumerator CreateRoutine()
            {
                yield return new WaitForEndOfFrame();
                
                if (statsProvider.TryGetStat(StatType.ExtraProjectilesFront, out var extraProjectilesFront))
                    Create(ProjectileSpawnType.Front, 1 + (int)extraProjectilesFront.Value);
                else
                    Create(ProjectileSpawnType.Front, 1);

                if (statsProvider.TryGetStat(StatType.ExtraProjectilesBack, out var extraProjectilesBack))
                    Create(ProjectileSpawnType.Back, (int)extraProjectilesBack.Value);

                if (statsProvider.TryGetStat(StatType.ExtraProjectilesSide, out var extraProjectilesSide))
                    Create(ProjectileSpawnType.Side, (int)extraProjectilesSide.Value);

                if (statsProvider.TryGetStat(StatType.ExtraProjectilesDiagonal, out var extraProjectilesDiagonal))
                    Create(ProjectileSpawnType.Diagonal, (int)extraProjectilesDiagonal.Value);

                foreach (var projectile in projectiles)
                    SetupProjectileAttributes(projectile, damageData);
                
                result.Invoke(projectiles);
            }
        }

        private void Create(ProjectileSpawnType spawnType, int count)
        {
            var spawnPoints = spawnHelper.GetSpawnPoints(spawnType, count);
            
            for (int i = 0; i < count; i++)
            {
                var projectile = _poolService.Spawn(projectilePrefab, spawnPoints[i].position, spawnPoints[i].rotation);
                projectile.transform.SetParent(null);
                projectile.SetSpawnType(spawnType);
                projectile.SetupDespawn(SetDespawnInPool(projectile));
                projectiles.Add(projectile);
            }
        }
        
        private Action<Projectile> SetDespawnInPool(Projectile projectile)
        {
            return projectile1 => _poolService.Despawn(projectile);
        }
        
        private void SetupProjectileAttributes(IProjectile projectile, IDamageData damageData)
        {
            setupVisitor.SetDamageData(damageData);

            var attributes = projectilePrefab.Attributes.Select(attribute => attribute.Copy()).ToList();

            foreach (var attribute in attributes)
            {
                attribute.Initialize(projectile);
                attribute.AcceptVisitor(setupVisitor);
            }

            projectile.SetAttributes(attributes);
        }

    }
}