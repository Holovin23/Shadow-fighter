using System.Collections.Generic;

namespace GameModules.Gameplay.Scripts.Characters.Stats
{
    public interface IStatsProvider
    {
        public bool AddStat(Stat stat);
        public void AddStats(IStatsProvider statsProvider, string source);
        public Stat GetStat(StatType type);
        public IEnumerable<Stat> GetAllStats();
        public bool HasStat(StatType type);
        public bool TryGetStat(StatType type, out Stat stat);
        public void AddModifier(StatModifier statModifier);
        public void RemoveModifier(StatModifier statModifier);
    }
}