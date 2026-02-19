using Session.Scheme.Block;
using Session.Scheme.Variables;

namespace Session.Scheme
{
    public class SchemeExecutionService : IActionProvider
    {
        public SchemeExecutionService(SchemeBlockFactory blockFactory, VariableService variableService, SchemeConsoleService consoleService)
        {
            _blockFactory = blockFactory;
            _variableService = variableService;
            _consoleService = consoleService;
        }

        private readonly SchemeBlockFactory _blockFactory;
        private readonly VariableService _variableService;
        private readonly SchemeConsoleService _consoleService;

        private IBlock _startBlock;
        private IBlock _endBlock;

        public IActionProvider Next { get => _startBlock; set => _startBlock = (IBlock)value; }

        public void StartProgramm()
        {
            foreach (var variable in _variableService.Variables)
            {
                _variableService.SetValueToVariable(variable.variableName, variable.GetStartValue());
            }

            _startBlock = _blockFactory.Blocks.Find(b => b.Facade.BlockName == "START_BLOCK");
            _endBlock = _blockFactory.Blocks.Find(b => b.Facade.BlockName == "END_BLOCK");
            _endBlock.Next = this;

            if (_startBlock == null)
            {
                _consoleService.SpawnMessage("У схемы не обнаружен стартовый блок!");
            }

            if (!_startBlock.CheckForCorrectRelationships())
            {
                _consoleService.SpawnMessage("У схемы не обнаружен конец! Корректно подключите все провода!");
            }

            else if (!_startBlock.CheckForCorrectValues())
            {
                _consoleService.SpawnMessage("У не указаны важные параметры!");
            }

            else
            {
                _consoleService.SpawnMessage("Программа запущена");
                Next.ProvideAction();
            }
        }

        public void ProvideAction()
        {
            _consoleService.SpawnMessage("Программа завершена");
        }
    }
}