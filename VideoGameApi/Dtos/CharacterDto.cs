using System.ComponentModel.DataAnnotations;
using VideoGameApi.Models;

namespace VideoGameApi.Dtos
{
    public class CharacterCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public CharacterRole Role { get; set; }
        public int VideoGameId { get; set; }
    }
    public class CharacterResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string VideoGameTitle { get; set; } = string.Empty;
        public int VideoGameId { get; set; }
    }
}
