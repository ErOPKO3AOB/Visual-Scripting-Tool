using Session.Scheme.Connector;
using Session.Scheme.Connector.Points;
using UnityEngine;

namespace Session.Scheme.Connector
{
    public class VisualConnection : MonoBehaviour
    {
        public ConnectionPoint From { get; private set; }
        public ConnectionPoint To { get; private set; }

        public ActionConnector Action { get; private set; }

        private LineRenderer _lr;
        private Transform _endPoint;

        void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            _lr.positionCount = 3;

            _endPoint = new GameObject("EndPoint").transform;
            _endPoint.SetParent(transform);

            var col = _endPoint.gameObject.AddComponent<CircleCollider2D>();
            col.radius = 0.15f;
            _endPoint.gameObject.layer = gameObject.layer;
        }

        public void Init(ConnectionPoint from)
        {
            From = from;
            UpdateLine(from.transform.position);
        }

        public void Bind(ActionConnector action)
        {
            Action = action;

            //From.OnMoved += UpdateFrom;
            //To.OnMoved += UpdateTo;
        }

        private void UpdateFrom()
        {
            UpdateLine(To.transform.position);
        }

        private void UpdateTo()
        {
            UpdateLine(To.transform.position);
        }

        public void Finish(ConnectionPoint to)
        {
            To = to;
            UpdateLine(to.transform.position);
        }

        public void UpdateLine(Vector3 endPos)
        {
            Vector3 start = From.transform.position;

            Vector3 mid = Mathf.Abs(start.x - endPos.x) > Mathf.Abs(start.y - endPos.y)
                ? new Vector3(endPos.x, start.y)
                : new Vector3(start.x, endPos.y);

            _lr.SetPosition(0, start);
            _lr.SetPosition(1, mid);
            _lr.SetPosition(2, endPos);

            _endPoint.position = endPos;
        }

        private void OnDestroy()
        {
            //if (From != null) From.OnMoved -= UpdateFrom;
            //if (To != null) To.OnMoved -= UpdateTo;
        }
    }
}