using UnityEngine;

namespace Session.Scheme.Windows
{
    public abstract class SettingsBaseWindowUI : MonoBehaviour
    {
        [SerializeField] private string _windowName;
        public string WindowName { get { return _windowName; } }

        public abstract void OnEndEdit();
    }
}