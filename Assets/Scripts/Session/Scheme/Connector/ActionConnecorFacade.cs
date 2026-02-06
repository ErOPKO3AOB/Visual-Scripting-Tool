using GlobalServices.ProjectLifetime;
using Session.Scheme.Block;
using Session.Scheme.Block.Button;
using UnityEngine;

namespace Session.Scheme.Connector
{
    [RequireComponent(typeof(LineRenderer))]
    public class ActionConnecorFacade : MonoBehaviour
    {
        public void ConstructManually(IActionProvider firstProvider, BlockConfigs blockConfigs)
        {
            _connector = new ActionConnector(firstProvider);
            _blockConfigs = blockConfigs;
        }

        private BlockConfigs _blockConfigs;

        private LineRenderer _lineRenderer;
        public LineRenderer LineRenderer => _lineRenderer;
        private ActionConnector _connector;

        private DraggableConnectorPoint _draggableConnectorPoint;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1,
                new Vector3(transform.position.x,
                transform.position.y - 1.5f,
                transform.position.z));

            _draggableConnectorPoint = Instantiate(_blockConfigs.DraggableConnectorPointPrefab, _lineRenderer.GetPosition(_lineRenderer.positionCount - 1), Quaternion.identity);
            _draggableConnectorPoint.ConstructManually(this);
        }

        public void OnConnected(IActionProvider secondProvider)
        {
            _connector.Connect(secondProvider);
        }

        private void OnDestroy()
        {
            //_connector.firstProvider.Next = null;
        }
    }
}