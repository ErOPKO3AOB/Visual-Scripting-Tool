using GlobalServices.ProjectLifetime;
using Session.Scheme.Block;
using Session.Scheme.Block.Button;
using UnityEngine;

namespace Session.Scheme.Connector
{
    [RequireComponent(typeof(LineRenderer))]
    public class ActionConnecorFacade : MonoBehaviour
    {
        public void ConstructManually(IBlock block, BlockConfigs blockConfigs, Vector3 startOffset)
        {
            _block = block;
            _connector = new ActionConnector(_block);
            _blockConfigs = blockConfigs;
            _startOffset = startOffset;
        }

        private IBlock _block;
        private ActionConnector _connector;
        private BlockConfigs _blockConfigs;
        private Vector3 _startOffset;

        private LineRenderer _lineRenderer;
        public LineRenderer LineRenderer => _lineRenderer;

        private DraggableConnectorPoint _draggableConnectorPoint;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, _startOffset);

            _draggableConnectorPoint = Instantiate(_blockConfigs.DraggableConnectorPointPrefab, transform);
            _draggableConnectorPoint.ConstructManually(this, _block);

            _draggableConnectorPoint.transform.SetParent(transform, false);
        }

        public void OnConnected(IBlock secondProvider)
        {
            _draggableConnectorPoint.gameObject.SetActive(false);

            _connector.Connect(secondProvider);
        }

        public void OnDisconnected()
        {
            _connector.Disconnect();
        }
    }
}