using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace VideoGameApi.Models
{
    public enum CharacterRole
    {
        Protagonist,
        Antagonist,
        Deuteragonist,
        SideCharacter,
        NPC
    }
    public class Character
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public CharacterRole Role { get; set; }
        public int VideoGameId { get; set; }

        [JsonIgnore]
        public VideoGame? VideoGame { get; set; }
    }
}
