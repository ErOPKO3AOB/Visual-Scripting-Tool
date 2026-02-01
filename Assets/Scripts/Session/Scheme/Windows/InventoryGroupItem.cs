using GlobalServices.ProjectLifetime;
using Session.Scheme.Block;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Session.Scheme.Windows
{
    public class InventoryGroupItem : BaseWindow
    {
        private class InventoryItem
        {
            public InventoryItem(string blockName, Button spawnButton)
            {
                _blockName = blockName;
                _spawnButton = spawnButton;

                _spawnButton.onClick.AddListener(ButtonClicked);
            }

            private readonly string _blockName;
            private readonly Button _spawnButton;

            public UnityAction<string> OnClicked;

            private void ButtonClicked()
            {
                OnClicked?.Invoke(_blockName);
            }

            ~InventoryItem()
            {
                _spawnButton.onClick.RemoveListener(ButtonClicked);
                OnClicked = null;
            }
        }

        [Inject]
        public void Construct(SchemeBuilderService schemeBuilderService, BlockConfigs blockConfigs)
        {
            _schemeBuilderService = schemeBuilderService;
            _blockConfigs = blockConfigs;
        }

        private SchemeBuilderService _schemeBuilderService;
        private BlockConfigs _blockConfigs;

        [SerializeField] private List<InventoryItem> _inventoryItems = new();
        [SerializeField] private Transform _content;

        private void Start()
        {
            for (int i = 0; i < _blockConfigs.BlockFacades.Length; i++)
            {
                SchemeBlockFacade facade = Instantiate(_blockConfigs.BlockFacades[i], _content);
                _inventoryItems.Add(new InventoryItem(_blockConfigs.BlockFacades[i].BlockName, facade.gameObject.AddComponent<Button>()));
                _inventoryItems[i].OnClicked += SpawnBlockFromInventory;
            }
        }

        private void SpawnBlockFromInventory(string blockName)
        {
            _schemeBuilderService.SpawnBlock(blockName);
        }
    }
}