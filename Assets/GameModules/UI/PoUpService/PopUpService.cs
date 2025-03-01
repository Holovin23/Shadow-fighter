using System.Collections.Generic;
using UnityEngine;

namespace TFPlay.Modules.UI.PopUpService
{
    public class PopUpService
    {
        private Dictionary<PopUpType, PopUp> _popUps = new();
        private PopUp _currentPopUp;
        private Queue<PopUp> _popUpQueue = new();

        public void Register(PopUp popUp, PopUpType type)
        {
            _popUps[type] = popUp;
        }

        public void ShowPopUp(PopUpType popUpType)
        {
            if (!_popUps.ContainsKey(popUpType))
            {
                Debug.LogError($"No {popUpType} registered");
                return;
            }

            _popUpQueue.Enqueue(_popUps[popUpType]);
            CheckShowNext();
        }

        public void PopupClosed()
        {
            _currentPopUp = null;
            CheckShowNext();
        }

        private void CheckShowNext()
        {
            if (_currentPopUp || _popUpQueue.Count == 0)
                return;

            _currentPopUp = _popUpQueue.Dequeue();
            _currentPopUp.Show();
        }
    }
}