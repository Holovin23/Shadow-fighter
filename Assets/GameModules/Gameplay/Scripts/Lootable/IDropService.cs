using UnityEngine;

namespace GameModules.Gameplay.Scripts.Lootable
{
    public interface IDropService 
    {
        public void SpawnDrop(DropType dropType,Vector3 position, int count);
    }
}