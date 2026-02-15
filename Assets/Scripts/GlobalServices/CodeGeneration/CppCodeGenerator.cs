using System.Threading.Tasks;

namespace GlobalServices.CodeGeneration
{
    public class CppCodeGenerator : ICodeGenerator
    {
        public void GatherBlocks()
        {
            throw new System.NotImplementedException();
        }

        public Task MakeStringConditionCodeParts()
        {
            throw new System.NotImplementedException();
        }

        public Task MakeStringInitializedVariable()
        {
            throw new System.NotImplementedException();
        }

        public Task MakeStringInputCodeParts()
        {
            throw new System.NotImplementedException();
        }

        public Task MakeStringActionCodeParts()
        {
            throw new System.NotImplementedException();
        }

        public Task MakeStringOutputCodeParts()
        {
            throw new System.NotImplementedException();
        }

        Task<string> ICodeGenerator.Generate()
        {
            throw new System.NotImplementedException();
        }
    }
}