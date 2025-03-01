using UnityEngine;

namespace TFPlay.Modules.GameResources
{
    [System.Serializable]
    public class ResourceConfigData
    {
        public ResourceType Type;
        public string Name;
        public Sprite Icon;
        public Color Color = Color.white;
    }
}