using Session.Scheme.Block;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private string _windowName;
        [SerializeField] private bool _multipleInstance;
        
        protected object sender;
        
        public string WindowName => _windowName;
        public bool MultipleInstance => _multipleInstance;
        
        public virtual void SetSender(object sender)
        {
            this.sender = sender;
        }
    }
}