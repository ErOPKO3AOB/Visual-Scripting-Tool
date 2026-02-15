using Session.Scheme.Block;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private string _windowName;
        [SerializeField] private bool _singleInstance;
        
        protected object sender;
        
        public string WindowName => _windowName;
        public bool SingleInstance => _singleInstance;

        public virtual void SetSender(object sender)
        {
            this.sender = sender;
        }
    }
}