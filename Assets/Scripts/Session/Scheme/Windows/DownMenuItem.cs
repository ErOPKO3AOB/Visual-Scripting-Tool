using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class DownMenuItem : BaseWindow
    {
        [Inject]
        public void Construct(SchemeBlockFactory schemeBuilderService, BlockConfigs blockConfigs, WindowFactory windowService)
        {
            _schemeBuilderService = schemeBuilderService;
            _blockConfigs = blockConfigs;
            _windowService = windowService;
        }

        private SchemeBlockFactory _schemeBuilderService;
        private BlockConfigs _blockConfigs;
        private WindowFactory _windowService;

        [Header("Inventory")]
        [SerializeField] private List<InventoryBlockItem> _inventoryItems = new();
        [SerializeField] private Transform _inventoryContent;

        [Header("Buttons")]
        [SerializeField] private Button _BUTTON_NOT_ASSIGNED;
        [SerializeField] private Button _consoleButton;
        [SerializeField] private Toggle _deleteToggle;

        private void Start()
        {
            _consoleButton.onClick.AddListener(() =>
            {
                _windowService.OpenWindow("CONSOLE_WINDOW");
            });

            _deleteToggle.onValueChanged.AddListener((value) =>
            {
                _schemeBuilderService.MakeAllBlocksWaitForDestroying(value);
            });

            for (int i = 0; i < _blockConfigs.BlockFacades.Count; i++)
            {
                if (!_blockConfigs.BlockFacades[i].SingleInstance)
                {
                    InventoryBlockItem inventoryItem = (InventoryBlockItem)_windowService.OpenWindow("INVENTORY_BLOCK_ITEM", _inventoryContent, this);

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
                            type = typeof(MethodBlock);
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

            _consoleButton.onClick.RemoveAllListeners();

            _deleteToggle.onValueChanged.RemoveAllListeners();
        }
    }
}