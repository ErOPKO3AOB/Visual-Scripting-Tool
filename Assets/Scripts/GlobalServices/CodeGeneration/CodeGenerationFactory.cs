using Cysharp.Threading.Tasks;
using GlobalServices.CodeGeneration;
using Session.Scheme;
using Session.Scheme.Block;
using Session.Scheme.Block.Types;
using Session.Scheme.Variables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace GlobalServices
{
    public class CodeGenerationFactory
    {
        public CodeGenerationFactory(VariableService variableService, SchemeBlockFactory schemeBlockFactory, SchemeConsoleService consoleService)
        {
            _variableService = variableService;
            _schemeBlockFactory = schemeBlockFactory;
            _consoleService = consoleService;
        }

        private readonly VariableService _variableService;
        private readonly SchemeBlockFactory _schemeBlockFactory;
        private readonly SchemeConsoleService _consoleService;

        public const int SMALL_OPERATION_DELAY_MS = 20;
        public const int MEDIUM_OPERATION_DELAY_MS = 100;

        public List<SchemeVariableBase> SchemeVariables { get; private set; } = new();

        public StartBlock StartBlock { get; private set; }
        public EndBlock EndBlock { get; private set; }
        public List<ActionBlock> MethodBlocks { get; private set; } = new();
        public List<ConditionBlock> ConditionBlocks { get; private set; } = new();
        public List<OutputBlock> OutputBlocks { get; private set; } = new();
        public List<InputBlock> InputBlocks { get; private set; } = new();

        private CancellationTokenSource _cts;

        public async UniTask GatherBlocks()
        {
            _cts = new CancellationTokenSource();

            SchemeVariables = _variableService.Variables;
            await UniTask.Delay(SMALL_OPERATION_DELAY_MS, cancellationToken: _cts.Token);

            StartBlock = null;
            StartBlock = (StartBlock)_schemeBlockFactory.Blocks.Find(b => b.GetType() == typeof(StartBlock));
            await UniTask.Delay(SMALL_OPERATION_DELAY_MS, cancellationToken: _cts.Token);

            EndBlock = null;
            EndBlock = (EndBlock)_schemeBlockFactory.Blocks.Find(b => b.GetType() == typeof(EndBlock));
            await UniTask.Delay(SMALL_OPERATION_DELAY_MS, cancellationToken: _cts.Token);

            MethodBlocks.Clear();
            var methodBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(ActionBlock));
            for (int i = 0; i < methodBlocks.Count; i++)
                MethodBlocks.Add((ActionBlock)methodBlocks[i]);
            await UniTask.Delay(MEDIUM_OPERATION_DELAY_MS, cancellationToken: _cts.Token);

            ConditionBlocks.Clear();
            var conditionBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(ConditionBlock));
            for (int i = 0; i < conditionBlocks.Count; i++)
                ConditionBlocks.Add((ConditionBlock)conditionBlocks[i]);
            await UniTask.Delay(MEDIUM_OPERATION_DELAY_MS, cancellationToken: _cts.Token);

            OutputBlocks.Clear();
            var outputBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(OutputBlock));
            for (int i = 0; i < outputBlocks.Count; i++)
                OutputBlocks.Add((OutputBlock)outputBlocks[i]);
            await UniTask.Delay(MEDIUM_OPERATION_DELAY_MS, cancellationToken: _cts.Token);

            InputBlocks.Clear();
            var inputBlocks = _schemeBlockFactory.Blocks.FindAll(b => b.GetType() == typeof(InputBlock));
            for (int i = 0; i < inputBlocks.Count; i++)
                InputBlocks.Add((InputBlock)inputBlocks[i]);
            await UniTask.Delay(MEDIUM_OPERATION_DELAY_MS, cancellationToken: _cts.Token);
        }

        // Новый метод для логирования ошибок
        public void LogError(string message)
        {
            _consoleService.SpawnMessage(message);
            _cts.Cancel();
            _cts.Dispose();
        }

        public async UniTask<string> GenerateCode(/*ICodeGenerator codeGenerator*/)
        {
            await GatherBlocks();

            if (StartBlock == null)
            {
                LogError("The starting block was not detected in the scheme!");
                return "Error! Details in console";
            }

            if (!StartBlock.CheckForCorrectRelationships())
            {
                LogError("The scheme has no end! Connect all the wires correctly!");
                return "Error! Details in console";
            }

            else if (!StartBlock.CheckForCorrectValues())
            {
                LogError("Important parameters are not specified in the scheme!");
                return "Error! Details in console";
            }

            ICodeGenerator codeGenerator = new CsharpCodeGenerator(this);
            string code = await codeGenerator.Generate();

            return code;
        }
    }
}