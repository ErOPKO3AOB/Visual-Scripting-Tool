using System.Threading.Tasks;

namespace GlobalServices.CodeGeneration
{
    public interface ICodeGenerator
    {
        Task<string> Generate();

    }
}