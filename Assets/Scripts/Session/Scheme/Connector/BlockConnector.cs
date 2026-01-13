using Session.Execution;
using Session.Scheme.Block;

namespace Session.Scheme.Connector
{
    public class BlockConnector : IExecutionUnit
    {
        private IActionProvider _inputConnection;
        private IActionProvider _outputConnection;

        public void SetInputConnection(IActionProvider connection)
        {
            _inputConnection = connection;
        }

        public void SetOutputConnection(IActionProvider connection)
        {
            _outputConnection = connection;
        }

        public void ProvideSignal()
        {

        }
    }
}