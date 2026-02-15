using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Session.Scheme;

namespace Session.Scheme.Windows
{
    public class SessionWindow : BaseWindow
    {
        [Inject]
        public void Construct(SchemeBlockFactory schemeBuilderService, BlockConfigs blockConfigs, WindowFactory windowFactory, SchemeExecutionService schemeExecutionService, SchemeConsoleService consoleService)
        {
            _schemeBuilderService = schemeBuilderService;
            _windowFactory = windowFactory;
            _blockConfigs = blockConfigs;
            _schemeExecutionService = schemeExecutionService;
            _consoleService = consoleService;
        }

        private SchemeBlockFactory _schemeBuilderService;
        private BlockConfigs _blockConfigs;
        private WindowFactory _windowFactory;
        private SchemeExecutionService _schemeExecutionService;
        // TODO: Implement notifications on button
        private SchemeConsoleService _consoleService;

        [Header("UI")]
        [SerializeField] private VariableListWindow _variableListWindowPrefab;
        [SerializeField] private ConsoleWindow _consoleWindowPrefab;
        [SerializeField] private InventoryBlockItem _inventoryBlockItemPrefab;
        [SerializeField] private CodeGenerationWindow _codeGenerationWindowPrefab;

        [Header("Inventory")]
        [SerializeField] private List<InventoryBlockItem> _inventoryItems = new();
        [SerializeField] private Transform _inventoryContent;

        [Header("Buttons")]
        [SerializeField] private Button _variableListButton;
        [SerializeField] private Button _startProgrammButton;
        [SerializeField] private Button _consoleButton;
        [SerializeField] private Toggle _deleteToggle;
        [SerializeField] private Button _codeGenerationButton;

        private void Start()
        {
            _variableListButton.onClick.AddListener(() =>
            {
                _windowFactory.OpenWindow(_variableListWindowPrefab);
            });

            _startProgrammButton.onClick.AddListener(() => 
            {
                _windowFactory.OpenWindow(_consoleWindowPrefab);
                _schemeExecutionService.StartProgramm();
            });

            _consoleButton.onClick.AddListener(() =>
            {
                _windowFactory.OpenWindow(_consoleWindowPrefab);
            });

            _deleteToggle.onValueChanged.AddListener((value) =>
            {
                _schemeBuilderService.MakeAllBlocksWaitForDestroying(value);
            });

            _codeGenerationButton.onClick.AddListener(() =>
            {
                _windowFactory.OpenWindow(_codeGenerationWindowPrefab);
            });

            for (int i = 0; i < _blockConfigs.BlockFacades.Count; i++)
            {
                if (!_blockConfigs.BlockFacades[i].SingleInstance)
                {
                    InventoryBlockItem inventoryItem = (InventoryBlockItem)_windowFactory.OpenWindow(_inventoryBlockItemPrefab, _inventoryContent, this);

                    Type type;
                    switch (i)
                    {
                        case 1:
                            type = typeof(InputBlock);
                            break;
                        case 2:
                            type = typeof(OutputBlock);
                            break;
                        case 3:
                            type = typeof(ConditionBlock);
                            break;
                        default:
                            type = typeof(ActionBlock);
                            break;
                    }

                    inventoryItem.ConstructManualy(type,
                        _blockConfigs.BlockFacades[i].BlockName,
                        _blockConfigs.BlockFacades[i].Label.GetText(),
                        _blockConfigs.BlockFacades[i].SpriteRenderer.color);
                    inventoryItem.OnPressed += SpawnBlockFromInventory;

                    _inventoryItems.Add(inventoryItem);
                }
            }
        }

        private void SpawnBlockFromInventory(string blockName)
        {
            _schemeBuilderService.SpawnBlock(blockName);
        }

        private void OnDestroy()
        {
            foreach (var item in _inventoryItems)
            {
                item.OnPressed -= SpawnBlockFromInventory;
            }

            _startProgrammButton.onClick.RemoveAllListeners();

            _consoleButton.onClick.RemoveAllListeners();

            _deleteToggle.onValueChanged.RemoveAllListeners();
        
            _codeGenerationButton.onClick.RemoveAllListeners();
        }
    }
}