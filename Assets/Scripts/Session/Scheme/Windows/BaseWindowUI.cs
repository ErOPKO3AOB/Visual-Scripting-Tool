using System;
using UnityEngine;

namespace Session.Scheme.Windows
{
    public abstract class BaseWindowUI : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private string _windowName;
        public string WindowName { get { return _windowName; } }
    }
}