using System;
using UnityEngine;
using User;

namespace Session.Scheme.Connector.Points
{
    public enum ConnectionPointType
    {
        Input,
        Output
    }

    public class ConnectionPoint : MonoBehaviour
    {
        public ConnectionPointType Type;

        public event Action OnOwnerMoved;

        void Awake()
        {
            var draggable = GetComponentInParent<DraggableObjectController>();
            if (draggable != null)
                draggable.OnDrag += () => OnOwnerMoved?.Invoke();
        }
    }
}