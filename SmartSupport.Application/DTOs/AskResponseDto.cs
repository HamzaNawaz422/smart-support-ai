namespace SmartSupport.Application.DTOs
{
    public class AskResponseDto
    {
        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;

        public bool IsCached { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}