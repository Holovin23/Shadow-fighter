using System.Collections.Generic;
using System.Linq;
using GameModules.Gameplay.Scripts.Characters.Stats;
using UnityEngine;

namespace Core.Upgrades
{
    [CreateAssetMenu(menuName = DataPath.Upgrades + nameof(UpgradeData), fileName = nameof(UpgradeData))]
    public class UpgradeData : ScriptableObject
    {
        public string id;
        public string title;
        [TextArea(3, 4)] public string info;
        public Sprite icon;
        public List<StatModifier> modifiers = new List<StatModifier>();

        public bool HasModifiers(List<StatModifier> inputModifiers)
        {
            foreach (var modifier in modifiers)
            {
                foreach (var inputModifier in inputModifiers)
                {
                    if (modifier.statType == inputModifier.statType)
                        return true;
                }
            }

            return false;
        }
    }
}