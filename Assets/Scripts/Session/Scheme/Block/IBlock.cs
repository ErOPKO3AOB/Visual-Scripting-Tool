namespace Session.Scheme.Block
{
    public interface IBlock
    {
        public bool SingleInstance { get; }

        public IBlock Next { get; set; }

        public SchemeBlockFacade Facade { get; }

        void ProvideAction();
    }
}