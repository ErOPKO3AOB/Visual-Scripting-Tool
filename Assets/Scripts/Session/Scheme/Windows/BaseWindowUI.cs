using System;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public abstract class BaseWindowUI : MonoBehaviour
    {
        [SerializeField] private string _windowName;
        public string WindowName { get { return _windowName; } }

        public abstract void OnEndEdit();
    }
}