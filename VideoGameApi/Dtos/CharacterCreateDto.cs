namespace VideoGameApi.Dtos
{
    public class CharacterCreateDto
    {
            public string Name { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public int VideoGameId { get; set; }
    }
}
