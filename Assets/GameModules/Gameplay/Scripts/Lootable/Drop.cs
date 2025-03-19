using System;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Lootable
{
    public class Drop : MonoBehaviour
    {
        private Action DespawnAction;
        protected int _count;


        public void SetCount(int count)
        {
            _count = count;
        }
        public void SetOnDespawn(Action onDespawn)
        {
            DespawnAction = onDespawn;
        }
        
        protected virtual void OnSpawn()
        {
            
        }        
        protected virtual void OnDespawn()
        {
            DespawnAction?.Invoke();
            DespawnAction = null;
        }
        protected virtual void OnPickUp()
        {
            
        }
    }
}