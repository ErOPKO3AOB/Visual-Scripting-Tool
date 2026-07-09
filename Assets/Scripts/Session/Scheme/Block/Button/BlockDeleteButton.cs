using User;

namespace Session.Scheme.Block.Button
{
    public class BlockDeleteButton : BaseBlockButton
    {
        public void ConstructManualy(SchemeBlockFactory blockFactory, IBlock block, WorldMouseControllerService worldUIControllerService)
        {
            _blockFactory = blockFactory;
            _block = block;
            _worldUIControllerService = worldUIControllerService;
        }

        private SchemeBlockFactory _blockFactory;
        private IBlock _block;
        private WorldMouseControllerService _worldUIControllerService;

        public override void Use()
        {
            _worldUIControllerService.OnStopInteractCallback += InteractionStopped;
        }

        private void InteractionStopped(BaseBlockButton blockButton)
        {
            _blockFactory.DestroyBlock(_block);
        }

        private void OnDestroy()
        {
            if (_worldUIControllerService != null)
                _worldUIControllerService.OnStopInteractCallback -= InteractionStopped;
        }
    }
}