namespace Session.Scheme.Block.Button
{
    public class BlockDeleteButton : BaseBlockButton
    {
        public void ConstructManualy(SchemeBlockFactory blockFactory, IBlock block)
        {
            _blockFactory = blockFactory;
            _block = block;
        }

        private SchemeBlockFactory _blockFactory;
        private IBlock _block;

        public override void Use()
        {
            _blockFactory.DestroyBlock(_block.Facade);
        }
    }
}