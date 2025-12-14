using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Models
{
    public class VideoGame
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Platform { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
        public bool IsDeleted { get; set; } = false;

    }
}
