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
    public class VideoGameResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Platform { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }

        public List<CharacterResponseDto> Characters { get; set; } = new List<CharacterResponseDto>();
    }
}
