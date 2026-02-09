using GlobalServices.ProjectLifetime;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;
using User;
using VContainer;
using VContainer.Unity;

namespace Session.Scheme
{
    public class SchemeBlockFactory
    {
        public SchemeBlockFactory(IObjectResolver objectResolver, BlockConfigs blockConfigs, VariableService variableService, CameraControllerFacade cameraControllerFacade)
        {
            _objectResolver = objectResolver;
            _blockConfigs = blockConfigs;
            _variableService = variableService;
            _cameraControllerFacade = cameraControllerFacade;
        }

        private readonly IObjectResolver _objectResolver;
        private readonly BlockConfigs _blockConfigs;
        private readonly VariableService _variableService;
        private readonly CameraControllerFacade _cameraControllerFacade;

        private List<IBlock> _blocks = new();

        public SchemeBlockFacade SpawnBlock(string blockName)
        {
            Debug.Log("Camera transform pos: " + _cameraControllerFacade.transform.position);

            // Spawning block
            var schemeBlockFacade = _objectResolver.Instantiate(
                // Finding prefab by name
                _blockConfigs.BlockFacades.Find(b => b.BlockName == blockName)
                .gameObject, 
                null,
                worldPositionStays: true)
                // Getting component
                .GetComponent<SchemeBlockFacade>();

            schemeBlockFacade.Rigidbody.position = _cameraControllerFacade.transform.position;
            
            // Searching for block type
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

            return schemeBlockFacade;
        }

        public void DestroyBlock(string blockName)
        {
            IBlock block = _blocks.Find(b => b.Facade.BlockName == blockName);
            _blocks.Remove(block);
            GameObject.Destroy(block.Facade.gameObject);
        }
    }
}