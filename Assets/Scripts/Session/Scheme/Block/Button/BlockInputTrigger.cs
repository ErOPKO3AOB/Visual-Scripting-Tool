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

            _worldUIControllerService.OnStopInteract += OnStopWorldUIInteraction;
        }

        private void OnStopWorldUIInteraction(BaseBlockButton baseBlockButton)
        {
            if (baseBlockButton is DraggableConnectorPoint draggableConnectorPoint && this != draggableConnectorPoint.Block.Facade.BlockInputTrigger)
            {
                Debug.DrawRay(transform.position, Vector2.up, Color.red);

                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, 0.5f, Vector2.up);
                
                if (hitInfo.collider.gameObject.GetComponent<DraggableConnectorPoint>() == draggableConnectorPoint)
                {
                    Debug.Log("CONNECTED!");
                    SchemeBlockFacade facade = draggableConnectorPoint.Block.Facade;
                    // TODO: ядекюрэ бшвхякемхе хмдейяю б гюбхяхлнярх нр рнцн, хг йюйни хлеммн юсроср рнвйх ашк бшбедем йнммейрнп
                    facade.BlockOutputButtons[0].ActionConnecorFacade.OnConnected(_block);
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
            _worldUIControllerService.OnStopInteract -= OnStopWorldUIInteraction;
        }
    }
}