using System.ComponentModel.DataAnnotations;

namespace WorkServices.API.Contracts.AI;

public sealed class GeneratePromptRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(1000)]
    public string Prompt { get; set; } = string.Empty;
}