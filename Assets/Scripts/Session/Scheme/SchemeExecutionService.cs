using UnityEngine;
using VContainer.Unity;

namespace Session.Scheme
{
    public class SchemeExecutionService : IInitializable
    {
        public SchemeExecutionService(SchemeBlockFactory blockFactory, SchemeConsoleService consoleService)
        {
            _blockFactory = blockFactory;
            _consoleService = consoleService;
        }

        private readonly SchemeBlockFactory _blockFactory;
        private readonly SchemeConsoleService _consoleService;

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void StartProgramm()
        {
            var startBlock = _blockFactory.Blocks.Find(b => b.Facade.BlockName == "START_BLOCK");
            if (!startBlock.CheckForCorrectRelationships())
            {
                _consoleService.SpawnMessage("У схемы не обнаружен конец! Корректно подключите все провода!");
            }

            else if (!startBlock.CheckForCorrectValues())
            {
                _consoleService.SpawnMessage("У не указаны важные параметры!");
            }

            else
            {
                _consoleService.SpawnMessage("Программа запущена!");
                startBlock.ProvideAction();
            }
        }
    }
}