using System;

namespace GameModules.Gameplay.Scripts.Characters
{
    [Serializable]
    public class AnimationData
    {
        public string name;
        public float transitionTime = 0.2f;
        public int layer = 0;
        public TransitionType transitionType = TransitionType.FixedTime;
    }

    public enum TransitionType
    {
        None = 0,
        NormalizedTime = 1,
        FixedTime = 2,
    }
}