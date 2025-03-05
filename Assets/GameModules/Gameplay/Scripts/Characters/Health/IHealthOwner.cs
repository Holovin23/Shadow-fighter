namespace GameModules.Gameplay.Scripts.Characters.Health
{
    public interface IHealthOwner
    {
        IHealthProvider HealthProvider { get; }
    }
}