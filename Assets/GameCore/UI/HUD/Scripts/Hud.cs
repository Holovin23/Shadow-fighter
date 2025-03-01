using System.Collections.Generic;
using UnityEngine;

namespace TFPlay.UI
{
    public class Hud : BaseUI, IHud
    {
        [SerializeField] private List<BaseButton> _buttons;

        public override void Initialize()
        {
            base.Initialize();

            foreach (var item in _buttons)
                item.Initialize();
        }
    }
}