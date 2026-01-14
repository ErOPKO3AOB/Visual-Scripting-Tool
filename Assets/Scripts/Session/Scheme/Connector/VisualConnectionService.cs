using Session.Scheme.Connector.Points;
using System.Collections.Generic;
using UnityEngine;

namespace Session.Scheme.Connector
{
    public class VisualConnectionService
    {
        private readonly Camera _camera;
        private readonly VisualConnection _prefab;

        private VisualConnection _current;

        private readonly List<VisualConnection> _connections = new();

        public VisualConnectionService(Camera camera, VisualConnection prefab)
        {
            _camera = camera;
            _prefab = prefab;
        }

        public void StartConnection(ConnectionPoint from)
        {
            if (from.Type != ConnectionPointType.Output)
                return;

            _current = Object.Instantiate(_prefab);
            _current.Init(from);
        }

        public void Tick()
        {
            if (_current == null)
                return;

            Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            _current.UpdateLine(pos);

            if (Input.GetMouseButtonUp(0))
            {
                TryFinishCurrent();
                _current = null;
            }
        }

        private void TryFinishCurrent()
        {
            var hit = Physics2D.OverlapPoint(_current.transform.GetChild(0).position);
            if (hit == null)
                return;

            var to = hit.GetComponent<ConnectionPoint>();
            if (to == null || to.Type != ConnectionPointType.Input)
                return;

            //// Создаём логический коннектор
            //var action = new ActionConnector(
            //    _current.From.GetComponentInPa<ActionNode>(),
            //    to.GetComponentInParent<ActionNode>()
            //);

            //_current.Bind(action);
            _current.Finish(to);

            _connections.Add(_current);
        }
    }
}
