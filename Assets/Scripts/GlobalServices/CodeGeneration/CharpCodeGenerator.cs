using Session.Scheme;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;

namespace GlobalServices.CodeGeneration
{
    public class CharpCodeGenerator : ICodeGenerator
    {
        public CharpCodeGenerator(SchemeBlockFactory schemeBlockFactory)
        {
            _schemeBlockFactory = schemeBlockFactory;
        }

        private readonly SchemeBlockFactory _schemeBlockFactory;

        private const string START_PROGRAMM_TEXT = 
            "\nvoid main()" +
            "\n{" +
            "\n" +
            "\n}";

        StartBlock _startBlock;
        EndBlock _endBlock;
        MethodBlock[] _methodBlocks;
        ConditionBlock[] _conditionBlocks;
        OutputBlock[] _outputBlocks;
        InputBlock[] _inputBlocks;

        public void GatherBlocks()
        {

        }

        public string Generate()
        {
            return null;
        }
    }
}