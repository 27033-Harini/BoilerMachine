namespace BoilerMachine.Models
{
    /// <summary>
    /// Operation result class to handle returning success or failure with error message
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="isOperationCompleted">Indicates the completion of a certain operation</param>
        /// <param name="errorMessage">Error message to be displayed upon failure</param>
        public OperationResult(bool isOperationCompleted, string? errorMessage = null)
        {
            this.IsOperationCompleted = isOperationCompleted;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets or sets the operation completed boolean value
        /// </summary>
        /// <value>Indicates the operation completion</value>
        public bool IsOperationCompleted { get; set; }

        /// <summary>
        /// Gets or sets the error message for a failure state completion status
        /// </summary>
        /// <value>Indicates the error message to be displayed</value>
        public string? ErrorMessage { get; set; }
    }
}
