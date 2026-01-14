using Session.Scheme.Block;
using UnityEngine;

namespace Session.Scheme.Connector
{
    public class ActionConnecorFacade : MonoBehaviour
    {
        private ActionConnector _connector;

        public void InitializeView(IActionProvider firstProvider)
        {
            _connector = new ActionConnector(firstProvider);
        }

        public void OnConnected(IActionProvider secondProvider)
        {
            _connector.Connect(secondProvider);
        }

        private void OnDestroy()
        {
            _connector.firstProvider.Next = null;
        }
    }
}