using Session.Scheme.Block;

namespace Session.Scheme
{
    public class SchemeExecutionService
    {
        IBlock _startProvider;

        public void StartProgramm()
        {
            _startProvider.ProvideAction();
        }
    }
}