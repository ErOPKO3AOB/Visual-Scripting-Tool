using GlobalServices.CodeGeneration;
using GlobalServices.CodeGeneration.ConstModules;
using System.Linq;

namespace GlobalServices
{
    public class CodeGenerationFactory
    {
        public string GenerateCode(ICodeGenerator codeGenerator)
        {
            string code = string.Empty;

            code = codeGenerator.Generate();

            return code;
        }

        public string PasteCodeIntoBody(string startCode, string codeToPaste)
        {
            int insertIndex = startCode.IndexOf(CodeModulesConsts.OPENED_CURLY_BRACE);
            if (insertIndex == -1)
            {
                return startCode;
            }

            string result = startCode.Substring(0, insertIndex + 1)
                            + codeToPaste
                            + startCode.Substring(insertIndex + 1);
            return result;
        }
    }
}