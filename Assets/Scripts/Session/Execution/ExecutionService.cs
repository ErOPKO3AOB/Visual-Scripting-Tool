using System;
using System.Collections.Generic;

namespace Session.Execution
{
    public class ExecutionService : IDisposable
    {
        public ExecutionService(ExecutionGraph graph)
        {
            _graph = graph;
        }

        private readonly ExecutionGraph _graph;
        public List<IExecutionUnit> ExecutionUnits { get; private set; }


        public void ExecuteProgramm()
        {

        }

        void IDisposable.Dispose()
        {
            // Some deinitialization logic
        }
    }
}