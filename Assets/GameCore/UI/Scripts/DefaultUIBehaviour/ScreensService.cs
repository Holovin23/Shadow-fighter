using System.Collections.Generic;
using UnityEngine;

namespace NewUI
{
    public class ScreensService
    {
        public ScreensService()
        {
            _screens = new();
        }

        private Dictionary<ScreenID, ScreenUI> _screens;

        public void Register(ScreenUI defaultScreen)
        {
            if (_screens.ContainsKey(defaultScreen.ID))
            {
                Debug.LogError($"Screen is already {defaultScreen.ID} registered");
            }

            _screens[defaultScreen.ID] = defaultScreen;
        }

        public void ShowScreen(ScreenID defaultScreenID)
        {
            if (_screens.ContainsKey(defaultScreenID) == false)
            {
                Debug.LogError($"No {defaultScreenID} registered");
            }

            _screens[defaultScreenID].Show();
        }

        public void HideScreen(ScreenID defaultScreenID)
        {
            if (_screens.ContainsKey(defaultScreenID) == false)
            {
                Debug.LogError($"No {defaultScreenID} registered");
            }

            _screens[defaultScreenID].Hide();
        }

        public T GetScreen<T>() where T : ScreenUI
        {
            foreach (var screen in _screens.Values)
            {
                if (screen.GetType() == typeof(T))
                {
                    return screen as T;
                }
            }

            Debug.LogError($"No {typeof(T)} registered");
            return null;
        }

        public ScreenUI GetScreen(ScreenID defaultScreenID)
        {
            if (_screens.ContainsKey(defaultScreenID) == false)
            {
                Debug.LogError($"No {defaultScreenID} registered");
            }

            return _screens[defaultScreenID];
        }
    }
}