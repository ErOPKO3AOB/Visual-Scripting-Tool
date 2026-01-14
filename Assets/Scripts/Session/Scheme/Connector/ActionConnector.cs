using Session.Scheme.Block;

namespace Session.Scheme.Connector
{
    public class ActionConnector
    {
        public ActionConnector(IActionProvider firstProvider)
        {
            this.firstProvider = firstProvider;
        }

        public readonly IActionProvider firstProvider;

        public void Connect(IActionProvider secondProvider)
        {
            firstProvider.Next = secondProvider;
        }
    }
}