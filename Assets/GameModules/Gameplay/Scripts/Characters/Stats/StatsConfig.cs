using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Stats
{
    [CreateAssetMenu(menuName = "ScriptableObject/StatConfig", fileName = "StatsConfig")]
    public class StatsConfig : ScriptableObject, IStatsProvider
    {
        [SerializeField] private string id;
        [SerializeField] private List<Stat> stats = new List<Stat>();

        private Dictionary<StatType, Stat> statsByType;
        private Dictionary<StatType, Stat> StatByType => statsByType ??= stats.ToDictionary(x=> x.Type);

        public string ID => id;

        public bool AddStat(Stat stat)
        {
            if (StatByType.Values.FirstOrDefault(x => x.Type == stat.Type) != null)
            {
                Debug.LogError($"Something wrong: you trying to add stat with type <{stat.Type}> that already present in StatsConfig <{name}>.");
                return false;
            }

            stats.Add(stat);
            return StatByType.TryAdd(stat.Type, stat);
        }

        public void AddStats(IStatsProvider statsProvider, string source)
        {
            var allStats = statsProvider.GetAllStats();

            foreach (var stat in allStats)
            {
                if (TryGetStat(stat.Type, out var existStat))
                    existStat.AddModifier(new StatModifier(stat.Type, stat.Value, source: source));
                else
                    AddStat(stat);
            }
        }

        public bool TryGetStat(StatType type, out Stat stat)
        {
            stat = GetStat(type);
            return stat != null;
        }

        public void AddModifiers(List<StatModifier> itemsModifiers)
        {
            foreach (var modifier in itemsModifiers)
                AddModifier(modifier);
        }

        public void AddModifier(StatModifier statModifier)
        {
            if(statModifier == null)
                return;
            
            if (!HasStat(statModifier.statType))
                AddStat(new Stat(statModifier.statType, 0f));

            if (TryGetStat(statModifier.statType, out var stat))
                stat.AddModifier(statModifier);
        }

        public void RemoveModifiers(List<StatModifier> itemsModifiers)
        {
            foreach (var modifier in itemsModifiers)
                RemoveModifier(modifier);
        }

        public void RemoveModifier(StatModifier statModifier)
        {
            if(statModifier == null)
                return;
            
            if (TryGetStat(statModifier.statType, out var stat))
                stat.RemoveModifier(statModifier);
        }

        public IEnumerable<Stat> GetAllStats() =>
            stats.ToList();

        public bool HasStat(StatType type) =>
            GetStat(type) != null;

        public Stat GetStat(StatType type) =>
            StatByType.ContainsKey(type) ? StatByType[type] : null;

        public StatsConfig CreateCopy()
        {
            var newConfig = CreateInstance<StatsConfig>();
            
            newConfig.id = ID;
            
            var statsToCopy = GetAllStats();

            newConfig.stats = new List<Stat>();

            foreach (var stat in statsToCopy)
                newConfig.stats.Add(new Stat(stat));

            newConfig.statsByType = newConfig.stats.ToDictionary(x=> x.Type);
            
            return newConfig;
        }
    }
}