namespace Session.Scheme.Block
{
    public interface IBlock : IActionProvider
    {
        public enum BlockType
        {
            Start, Method, Output, Input, Condition, End
        }

        public BlockType ConcreteType { get; }

        public bool SingleInstance { get; }

        public SchemeBlockFacade Facade { get; }

        public int CurrentOutputIndex { get; set; }

        bool CheckForCorrectRelationships();

        bool CheckForCorrectValues();
    }
}