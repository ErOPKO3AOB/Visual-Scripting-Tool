using Cysharp.Threading.Tasks;

namespace GlobalServices.CodeGeneration
{
    public interface ICodeGenerator
    {
        UniTask<string> Generate();

    }
}