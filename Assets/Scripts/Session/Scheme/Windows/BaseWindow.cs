using Session.Scheme.Block;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private string _windowName;
        public string WindowName { get { return _windowName; } }
        protected IActionProvider sender;

        public void SetSender(IActionProvider sender)
        {
            this.sender = sender;

            CastSender();
        }

        protected abstract void CastSender();
    }
}