using GlobalServices.ProjectLifetime;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public class WindowService
    {
        public WindowService(BlockConfigs blockConfigs)
        {
            _blockConfigs = blockConfigs;
        }

        private readonly BlockConfigs _blockConfigs;

        public void OpenWindow(string windowName)
        {
            GameObject.Instantiate(SearchWindow(windowName));
        }

        public void CloseWindow(string windowName)
        {
            GameObject.Destroy(SearchWindow(windowName));
        }

        private GameObject SearchWindow(string windowName)
        {
            for (int i = 0; i < _blockConfigs.VariableItemPrefabs.Length; i++)
            {
                if (_blockConfigs.VariableItemPrefabs[i].WindowName == windowName)
                {
                    return _blockConfigs.VariableItemPrefabs[i].gameObject;
                }
            }

            return null;
        }
    }
}