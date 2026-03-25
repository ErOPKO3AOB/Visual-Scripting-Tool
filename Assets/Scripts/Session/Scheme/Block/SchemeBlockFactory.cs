using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using User;
using VContainer;
using VContainer.Unity;

namespace Session.Scheme.Block
{
    public class SchemeBlockFactory
    {
        public SchemeBlockFactory(IObjectResolver objectResolver, BlockConfigs blockConfigs)
        {
            _objectResolver = objectResolver;
            _blockConfigs = blockConfigs;
        }

        private readonly IObjectResolver _objectResolver;
        private readonly BlockConfigs _blockConfigs;

        public List<IBlock> Blocks { get; private set; } = new();

        public bool DestroyWaiting { get; private set; }

        public SchemeBlockFacade SpawnBlock(string blockName)
        {
            // Spawning block
            var schemeBlockFacade = _objectResolver.Instantiate(
                // Finding prefab by name
                _blockConfigs.BlockFacades.Find(b => b.BlockName == blockName)
                .gameObject,
                null,
                worldPositionStays: true)
                // Getting component
                .GetComponent<SchemeBlockFacade>();

            schemeBlockFacade.Collider.enabled = false;
            schemeBlockFacade.Rigidbody.MovePosition(_objectResolver.Resolve<CameraControllerFacade>().transform.position);
            schemeBlockFacade.Collider.enabled = true;
            schemeBlockFacade.Rigidbody.bodyType = RigidbodyType2D.Dynamic;

            // Searching for block type
            IBlock block = null;

            var variableService = _objectResolver.Resolve<VariableService>();
            var consoleService = _objectResolver.Resolve<SchemeConsoleService>();

            if (blockName == _blockConfigs.BlockFacades[0].BlockName)
                block = new ActionBlock(schemeBlockFacade, variableService);
            else if (blockName == _blockConfigs.BlockFacades[1].BlockName)
                block = new InputBlock(schemeBlockFacade, variableService, consoleService);
            else if (blockName == _blockConfigs.BlockFacades[2].BlockName)
                block = new OutputBlock(schemeBlockFacade, consoleService);
            else if (blockName == _blockConfigs.BlockFacades[3].BlockName)
                block = new ConditionBlock(schemeBlockFacade, variableService);
            else if (blockName == _blockConfigs.BlockFacades[4].BlockName)
                block = new StartBlock(schemeBlockFacade);
            else if (blockName == _blockConfigs.BlockFacades[5].BlockName)
                block = new EndBlock(schemeBlockFacade);

            schemeBlockFacade.Model = block;

            Blocks.Add(block);

            return schemeBlockFacade;
        }

        public void MakeAllBlocksWaitForDestroying(bool value)
        {
            DestroyWaiting = value;

            Blocks.ForEach((block) =>
            {
                if (!block.Facade.SingleInstance)
                    block.Facade.SetDestroyWaiting(DestroyWaiting);
            });
        }

        public void DestroyBlock(IBlock block)
        {
            IBlock blockToDestroy = Blocks.Find(b => b == block && b.Facade.BlockName == block.Facade.BlockName);
            if (blockToDestroy.SingleInstance) return;

            // Disconnect input point previous connections
            if (blockToDestroy.Facade.BlockInputTrigger.ConnectedActionConnectorFacade != null)
            {
                int index = blockToDestroy.Facade.BlockInputTrigger.ConnectedOutputButton != null ?
                    blockToDestroy.Facade.BlockInputTrigger.ConnectedOutputButton.Block.Facade.BlockOutputButtons.ToList().FindIndex(Ob => Ob == blockToDestroy.Facade.BlockInputTrigger.ConnectedOutputButton) :
                    0;
                blockToDestroy.Facade.BlockInputTrigger.ConnectedActionConnectorFacade.OnDisconnected(index);
            }

            // Disconnect all output points connections
            for (int i = 0; i < blockToDestroy.Facade.BlockOutputButtons.Length; i++)
            {
                if (blockToDestroy.Facade.BlockOutputButtons[i].ActionConnecorFacade != null)
                    blockToDestroy.Facade.BlockOutputButtons[i].ActionConnecorFacade.OnDisconnected(i);
            }

            Blocks.Remove(blockToDestroy);
            blockToDestroy.Dispose();
        }
    }
}