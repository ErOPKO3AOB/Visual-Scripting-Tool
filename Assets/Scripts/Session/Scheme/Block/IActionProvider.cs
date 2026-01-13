namespace Session.Scheme.Block
{
    public interface IActionProvider
    {
        bool CanMoveHere { get; protected set; }
        IActionProvider Next { get; protected set; }

        void ProvideAction();
    }
}