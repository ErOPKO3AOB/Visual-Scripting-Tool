using UnityEngine;
using User;

namespace Session.Scheme.Block.Button
{
    public class BlockInputTrigger : BaseBlockButton
    {
        public void ConstructManualy(IBlock block, WorldUIControllerService worldUIControllerService)
        {
            _block = block;
            _worldUIControllerService = worldUIControllerService;
        }

        private IBlock _block;
        private WorldUIControllerService _worldUIControllerService;

        protected override void Start()
        {
            base.Start();

            _worldUIControllerService.OnStopInteractCallback += OnStopWorldUIInteraction;
        }

        private void OnStopWorldUIInteraction(BaseBlockButton baseBlockButton)
        {
            if (baseBlockButton is DraggableConnectorPoint draggableConnectorPoint 
                && this != draggableConnectorPoint.Block.Facade.BlockInputTrigger)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, 0.25f, Vector3.forward);
                
                if (hitInfo.collider.gameObject.GetComponent<DraggableConnectorPoint>() == draggableConnectorPoint)
                {
                    SchemeBlockFacade facade = draggableConnectorPoint.Block.Facade;

                    int outputBlockIndex = 0;
                    for (int i = 0; i < draggableConnectorPoint.Block.Facade.BlockOutputButtons.Length; i++)
                    {
                        if (draggableConnectorPoint.Block.Facade.BlockOutputButtons[i] == facade.BlockOutputButtons[i])
                        {
                            outputBlockIndex = i;
                        }
                    }

                    facade.BlockOutputButtons[outputBlockIndex].ActionConnecorFacade.OnConnected(_block);
                }
            }
        }

        public override void Use()
        {

        }

        public void StopUsage()
        {

        }

        private void OnDestroy()
        {
            _worldUIControllerService.OnStopInteractCallback -= OnStopWorldUIInteraction;
        }
    }
}