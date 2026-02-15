namespace Session.Scheme.Block
{
    public interface IBlock : IActionProvider
    {
        public bool SingleInstance { get; }

        public SchemeBlockFacade Facade { get; }

        bool CheckForCorrectRelationships();

        bool CheckForCorrectValues();
    }
}