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
        public string Name { get; set; } = string.Empty;
        public CharacterRole Role { get; set; }
        public int VideoGameId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public VideoGame? VideoGame { get; set; }
    }
}
