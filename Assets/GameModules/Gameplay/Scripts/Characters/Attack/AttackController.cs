using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class AttackController : MonoBehaviour
    {
        [Inject] protected IColliderService _colliderService;
        
        protected Character _ownerCharacter;
        public virtual void Initialize(Character owner)
        {
            _ownerCharacter = owner;
        }
    }
}