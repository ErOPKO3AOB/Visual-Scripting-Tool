namespace Session.Execution
{
    public class ExecutionGraph
    {
        // STORES GRAPH OF ACTIONS
        public ExecutionElement[] executionElements;
    }

    public class ExecutionElement
    {
        public IExecutionUnit executionUnit;
        //public ExecutionElement
    }
}