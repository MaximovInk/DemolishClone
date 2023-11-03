using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaximovInk
{
    public class LayoutScreens : MonoBehaviourSingleton<LayoutScreens>
    {
        private LayoutScreen[] _screens;

        private Stack<LayoutScreen> _screensStack;

        private void Awake()
        {
            _screensStack = new Stack<LayoutScreen>();

           _screens = GetComponentsInChildren<LayoutScreen>(true);
        }

        public bool HasActiveScreens()
        {
            return _screensStack.Count > 0;
        }

        public void ShowScreen(string name)
        {
            var screen = _screens.FirstOrDefault(x=> x.gameObject.name.ToUpper() == name.ToUpper());

            if (screen == null)
            {
                Debug.LogError($"Screen: {name} not found!");
                return;
            }

            bool isNeedToShowScreen = true;

            for (int i = 0; i < _screens.Length; i++)
            {
                if (_screens[i] == screen) {

                    isNeedToShowScreen = !_screens[i].IsActive;
                    if(!isNeedToShowScreen)
                        Debug.LogError($"Screen {name} is active");

                    continue;
                }

                _screens[i].Hide();
            }

            if(isNeedToShowScreen)
            {
                _screensStack.Push(screen);
                screen.Show(false);
            }

        }

        public void OnScreenClosed()
        {
            if (_screensStack.Count == 0) return;

            //Current screen
            _screensStack.Pop();

            if (_screensStack.Count == 0) return;

            //Next in stack
            var next = _screensStack.Pop();

            next.Show(true);
        }
    }
}
