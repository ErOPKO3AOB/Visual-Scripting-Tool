using User;

namespace Session.Scheme.Block.Button
{
    public class BlockDeleteButton : BaseBlockButton
    {
        public void ConstructManualy(SchemeBlockFactory blockFactory, IBlock block, WorldUIControllerService worldUIControllerService)
        {
            _blockFactory = blockFactory;
            _block = block;
            _worldUIControllerService = worldUIControllerService;
        }

        private SchemeBlockFactory _blockFactory;
        private IBlock _block;
        private WorldUIControllerService _worldUIControllerService;

        public override void Use()
        {
            _worldUIControllerService.OnStopInteractCallback += InteractionStopped;
        }

        private void InteractionStopped(BaseBlockButton blockButton)
        {
            _blockFactory.DestroyBlock(_block.Facade);
        }

        private void OnDestroy()
        {
            if (_worldUIControllerService != null)
                _worldUIControllerService.OnStopInteractCallback -= InteractionStopped;
        }
    }
}