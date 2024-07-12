namespace Api;

public class ErrorResponseDto
{
    /// <summary>
    /// Gets or sets the status code
    /// </summary>
    public required int Status { get; set; }

    /// <summary>
    /// Gets the collection of errors
    /// </summary>
    public required ICollection<ErrorModel> Errors { get; init; }

    /// <summary>
    /// An individual error
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets the code of the error
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets a description of the error
        /// </summary>
        public required string Message { get; set; } = "An error occurred";
    }
}