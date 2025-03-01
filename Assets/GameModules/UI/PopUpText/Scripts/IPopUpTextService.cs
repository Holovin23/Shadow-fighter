using UnityEngine;

namespace TFPlay.Modules.PopUpText
{
    public interface IPopUpTextService
    {
        public void ShowPopUp(string text, Vector3 position);
        
        public void ShowPopUp(string text, Vector3 position, Vector2 size);
    }
}