using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Stats;

namespace GameModules.Gameplay.Scripts.Characters
{
    public interface IDamageGenerator
    {
        DamageData Generate(IStatsProvider statsProvider, IInteractionData interactionData);
    }
}