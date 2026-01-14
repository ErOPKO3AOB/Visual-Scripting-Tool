namespace Session.Scheme.Block
{
    public interface IActionProvider
    {
        IActionProvider Next { get; set; }

        void ProvideAction();
    }
}