using DG.Tweening;
using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectileAutoDespawnAttribute),
        fileName = nameof(ProjectileAutoDespawnAttribute))]
    public class ProjectileAutoDespawnAttribute : ProjectileAttribute
    {
        [SerializeField] private float delayToDespawn = 3f;

        private Tween delayedTween;
        
        public override void OnSpawn(IInteractionData interactionData)
        {
            base.OnSpawn(interactionData);
            delayedTween = DOVirtual.DelayedCall(delayToDespawn, projectile.OnDespawn).SetUpdate(false).Play();
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            delayedTween?.Kill();
        }
        
    }
}