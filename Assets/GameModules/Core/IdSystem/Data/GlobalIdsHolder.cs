using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TFPlay.Modules.Core.IdSystem
{
    [CreateAssetMenu(fileName = "GlobalIdsHolder", menuName = "24Play/IdSystem/GlobalIdsHolder", order = 1)]
    public class GlobalIdsHolder : ScriptableObject
    {
        public List<IdCategory> IdCategories;

#if UNITY_EDITOR
        [ContextMenu(nameof(UpdateIds))]
        public void UpdateIds()
        {
            IdCategories.Clear();
            
            var categories = Helpers.ProjectScriptableObjectFinder<IdCategory>.GetAllInstances();
            if (categories.Count > 0)
            {
                IdCategories.AddRange(categories);
            }
            
            EditorUtility.SetDirty(this);
        }
#endif
    }
}