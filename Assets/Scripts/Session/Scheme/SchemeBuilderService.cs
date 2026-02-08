using GlobalServices.ProjectLifetime;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Connector;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;
using User;
using VContainer.Unity;

namespace Session.Scheme
{
    public class SchemeBuilderService : IDisposable
    {
        public SchemeBuilderService(Func<string, Transform, SchemeBlockFacade> factory, BlockConfigs blockConfigs, VariableService variableService, CameraControllerFacade cameraControllerFacade)
        {
            _factory = factory;
            _blockConfigs = blockConfigs;
            _variableService = variableService;
            _cameraControllerFacade = cameraControllerFacade;
        }

        private readonly Func<string, Transform, SchemeBlockFacade> _factory;
        private readonly BlockConfigs _blockConfigs;
        private readonly VariableService _variableService;
        private readonly CameraControllerFacade _cameraControllerFacade;

        private List<IBlock> _blocks = new();
        private List<ActionConnector> _connectors = new();

        public void SpawnBlock(string blockName)
        {
            SchemeBlockFacade schemeBlockFacade = _factory.Invoke(blockName, _cameraControllerFacade.transform);

            IBlock block = null;

            if (blockName == _blockConfigs.BlockFacades[0].BlockName)
                block = new MethodBlock(schemeBlockFacade, _variableService);
            else if (blockName == _blockConfigs.BlockFacades[1].BlockName)
                block = new InputBlock(schemeBlockFacade, _variableService);
            else if (blockName == _blockConfigs.BlockFacades[2].BlockName)
                block = new OutputBlock(schemeBlockFacade/*, _variableService*/);
            else if (blockName == _blockConfigs.BlockFacades[3].BlockName)
                block = new ConditionBlock(schemeBlockFacade, _variableService);

            schemeBlockFacade.Model = block;

            _blocks.Add(block);
        }

        public void DestroyBlock(string blockName)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                //_blocks[i].Dispose();
                // Нужна инерфейсовая логика для блока типа IBlock
            }
        }

        public void ConnectBlocksWithConnector(IBlock outputPoint, IBlock inputPoint, ActionConnector connector)
        {
            //connector.SetInputConnection(outputPoint);
            //connector.SetOutputConnection(inputPoint);
        }

        public void SpawnConnector()
        {

        }

        void IDisposable.Dispose()
        {
            _blocks.Clear();
            _connectors.Clear();
        }
    }
}