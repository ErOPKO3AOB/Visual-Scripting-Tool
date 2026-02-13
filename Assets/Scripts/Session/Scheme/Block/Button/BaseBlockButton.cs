using Session.Scheme.Windows;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Session.Scheme.Block.Button
{
    [DisallowMultipleComponent]
    public abstract class BaseBlockButton : MonoBehaviour
    {
        protected virtual void Start()
        {
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }

        public abstract void Use();
    }
}