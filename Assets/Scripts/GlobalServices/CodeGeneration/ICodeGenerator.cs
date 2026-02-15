namespace GlobalServices.CodeGeneration
{
    public interface ICodeGenerator
    {
        void GatherBlocks();

        string Generate();
    }
}