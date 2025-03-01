using System.Collections.Generic;
using UnityEngine;

namespace TFPlay.Modules.Core.IdSystem
{
    [CreateAssetMenu(fileName = "IdCategory", menuName = "24Play/IdSystem/IdCategory", order = 1)]
    public class IdCategory : ScriptableObject
    {
        public List<string> Ids;
    }
}