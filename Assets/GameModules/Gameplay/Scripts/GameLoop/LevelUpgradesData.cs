using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Upgrades
{
    [Serializable]
    public class LevelUpgradesData
    {
        public int upgradeNumber;
        public int requireExp; 
        public List<UpgradeData> upgrades = new List<UpgradeData>();
        
        private List<UpgradeData> tempUpgrades = new List<UpgradeData>();
        
        public IEnumerable<UpgradeData> GetUpgrades(int count)
        {
            var allUpgrades = upgrades.ToList();

            for (int index = tempUpgrades.Count - 1; index >= 0; index--)
                allUpgrades.Remove(tempUpgrades[index]);
            
            var randomUpgrades = new List<UpgradeData>();

            for (int i = 0; i < count; i++)
                randomUpgrades.Add(allUpgrades.GetNotRepeatItem(ref tempUpgrades, count));

            return randomUpgrades;
        }
    }

}