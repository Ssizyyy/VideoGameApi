using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Dtos
{
    public class VideoGameCreateUpdateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Platform { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
    }
}
