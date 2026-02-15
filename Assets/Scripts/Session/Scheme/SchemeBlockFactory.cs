using GlobalServices.ProjectLifetime;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System.Collections.Generic;
using UnityEngine;
using User;
using VContainer;
using VContainer.Unity;

namespace Session.Scheme
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

            schemeBlockFacade.Rigidbody.position = _objectResolver.Resolve<CameraControllerFacade>().transform.position;

            // Searching for block type
            IBlock block = null;

            var variableService = _objectResolver.Resolve<VariableService>();
            var consoleService = _objectResolver.Resolve<SchemeConsoleService>();

            if (blockName == _blockConfigs.BlockFacades[0].BlockName)
                block = new MethodBlock(schemeBlockFacade, variableService);
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

        public void DestroyBlock(SchemeBlockFacade schemeBlockFacade)
        {
            IBlock block = Blocks.Find(b => b.Facade.BlockName == schemeBlockFacade.BlockName && b.Facade == schemeBlockFacade);
            if (block.SingleInstance) return;

            if (block.Facade.BlockInputTrigger.ConnectedActionConnectorFacade != null)
                block.Facade.BlockInputTrigger.ConnectedActionConnectorFacade.OnDisconnected();

            for (int i = 0; i < block.Facade.BlockOutputButtons.Length; i++)
            {
                if (block.Facade.BlockOutputButtons[i].ActionConnecorFacade != null)
                    block.Facade.BlockOutputButtons[i].ActionConnecorFacade.OnDisconnected();
            }

            Blocks.Remove(block);
            GameObject.Destroy(block.Facade.gameObject);
        }
    }
}