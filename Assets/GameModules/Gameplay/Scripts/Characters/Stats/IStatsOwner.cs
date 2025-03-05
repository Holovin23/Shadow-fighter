namespace GameModules.Gameplay.Scripts.Characters.Stats
{
    public interface IStatsOwner
    {
        IStatsProvider StatsProvider { get; }
    }
}