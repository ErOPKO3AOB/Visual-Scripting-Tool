using Session.Execution;

namespace Session.Scheme.Block
{
    public interface ISchemeBlock : IExecutionUnit
    {
        public void SendInput();
    }
}