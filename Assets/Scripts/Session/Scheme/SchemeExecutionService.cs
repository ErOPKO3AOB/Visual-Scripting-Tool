using Session.Scheme.Block;

namespace Session.Scheme
{
    public class SchemeExecutionService
    {
        IActionProvider _startProvider;

        public void StartProgramm()
        {
            _startProvider.ProvideAction();
        }
    }
}