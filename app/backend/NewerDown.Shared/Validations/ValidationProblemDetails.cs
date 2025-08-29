namespace NewerDown.Shared.Validations;

public class ValidationProblemDetails
{
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}