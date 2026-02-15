using System.Threading.Tasks;

namespace GlobalServices.CodeGeneration
{
    public interface ICodeGenerator
    {
        Task<string> Generate();

        Task MakeStringInitializedVariables();

        Task MakeStringMethodCodeParts();

        Task MakeStringConditionCodeParts();

        Task MakeStringInputCodeParts();

        Task MakeStringOutputCodeParts();
    }
}