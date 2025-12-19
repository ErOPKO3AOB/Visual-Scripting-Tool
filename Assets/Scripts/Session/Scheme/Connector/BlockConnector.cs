using Session.Execution;
using Session.Scheme.Block;

namespace Session.Scheme.Connector
{
    public class BlockConnector : IExecutionUnit
    {
        private ISchemeBlock _inputConnection;
        private ISchemeBlock _outputConnection;

        public void SetInputConnection(ISchemeBlock connection)
        {
            _inputConnection = connection;
        }

        public void SetOutputConnection(ISchemeBlock connection)
        {
            _outputConnection = connection;
        }

        public void ProvideSignal()
        {

        }
    }
}