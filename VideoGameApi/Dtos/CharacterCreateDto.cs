using VideoGameApi.Models;
using System.ComponentModel.DataAnnotations;

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
}
