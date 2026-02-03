using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Session.Scheme.Windows
{
    public class DownMenuItem : BaseWindow
    {
        [Inject]
        public void Construct(SchemeBuilderService schemeBuilderService, BlockConfigs blockConfigs, WindowService windowService)
        {
            _schemeBuilderService = schemeBuilderService;
            _blockConfigs = blockConfigs;
            _windowService = windowService;
        }

        private SchemeBuilderService _schemeBuilderService;
        private BlockConfigs _blockConfigs;
        private WindowService _windowService;

        [SerializeField] private List<InventoryBlockItem> _inventoryItems = new();
        [SerializeField] private Transform _inventoryContent;

        private void Start()
        {
            for (int i = 0; i < _blockConfigs.BlockFacades.Length; i++)
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

                inventoryItem.ConstructManualy(type, _blockConfigs.BlockFacades[i].BlockName, _blockConfigs.BlockFacades[i].BlockName);
                inventoryItem.OnPressed += SpawnBlockFromInventory;

                _inventoryItems.Add(inventoryItem);
            }
        }

        private void SpawnBlockFromInventory(string blockName)
        {
            _schemeBuilderService.SpawnBlock(blockName);
        }
    }
}