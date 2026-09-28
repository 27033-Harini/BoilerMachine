namespace BoilerMachine.Models
{
    public class OperationResult
    {
        public OperationResult(bool isOperationCompleted, string? errorMessage = null)
        {
            this.IsOperationCompleted = isOperationCompleted;
            this.ErrorMessage = errorMessage;
        }
        public bool IsOperationCompleted { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
