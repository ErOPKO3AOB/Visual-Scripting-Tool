using Session.Scheme.Connector;
using System.Linq;
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

        public ActionConnecorFacade ConnectedActionConnectorFacade { get; private set; }

        protected override void Start()
        {
            base.Start();

            _worldUIControllerService.OnStopInteractCallback += OnStopWorldUIInteraction;
        }

        private void OnStopWorldUIInteraction(BaseBlockButton baseBlockButton)
        {
            if (baseBlockButton == null && baseBlockButton is not DraggableConnectorPoint) return;

            if (baseBlockButton is DraggableConnectorPoint draggableConnectorPoint)
            {
                if (draggableConnectorPoint == null)
                    Debug.Log("draggableConnectorPoint is null");
                if (draggableConnectorPoint.Block == null)
                    Debug.Log("draggableConnectorPoint.Block is null");
                if (draggableConnectorPoint.Block.Facade == null)
                    Debug.Log("draggableConnectorPoint.Block.Facade is null");
                if (draggableConnectorPoint.Block.Facade.BlockInputTrigger == null)
                    Debug.Log("draggableConnectorPoint.Block.Facade.BlockInputTrigger is null");

                if (draggableConnectorPoint.Block.Facade.BlockInputTrigger == null
                    || this != draggableConnectorPoint.Block.Facade.BlockInputTrigger)
                {
                    Collider2D[] hitInfo = Physics2D.OverlapCircleAll(transform.position, 0.25f);

                    Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z), Color.red);

                    Collider2D correctCollider = hitInfo.ToList().Find(c => c.TryGetComponent<DraggableConnectorPoint>(out var connector) == draggableConnectorPoint);

                    if (correctCollider != null)
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
                        ConnectedActionConnectorFacade = facade.BlockOutputButtons[outputBlockIndex].ActionConnecorFacade;
                    }
                }
            }
        }

        public override void Use()
        {

        }

        private void OnDestroy()
        {
            _worldUIControllerService.OnStopInteractCallback -= OnStopWorldUIInteraction;

            ConnectedActionConnectorFacade = null;
        }
    }
}