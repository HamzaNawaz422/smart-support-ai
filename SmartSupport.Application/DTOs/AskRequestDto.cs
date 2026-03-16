using System.ComponentModel.DataAnnotations;

namespace SmartSupport.Application.DTOs
{
    public class AskRequestDto
    {
        [Required]
        [MinLength(3)]
        public string Question { get; set; } = string.Empty;
    }
}