using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoGameApi.Data;
using VideoGameApi.Dtos;
using VideoGameApi.Models;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController(VideoGameDbContext context) : ControllerBase
    {
        private readonly VideoGameDbContext _context = context;


        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetCharacters()
        {
            var characters = await _context.Characters
                //.Include(c => c.VideoGame)
                .ToListAsync();
            return Ok(characters);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacterById(int id)
        {
            var Character = await _context.Characters.FindAsync(id);
            if (Character == null)
                return NotFound("Character Not Found");
            return Ok(Character);
        }


        [HttpGet("ByGame/{gameId}")]
        public async Task<ActionResult<List<Character>>> GetCharactersByGameId(int gameId)
        {
            var characters = await _context.Characters
                .Where(c => c.VideoGameId == gameId)
                .ToListAsync();
            return Ok(characters);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCharacter(CharacterCreateDto request)
        {
            var game = await _context.VideoGames.FindAsync(request.VideoGameId);
            if (game == null)
                    return NotFound("Game Not Found");

            if (!Enum.TryParse<CharacterRole>(request.Role, true, out var parsedRole))
                return BadRequest("Invalid role value.");

            var newCharacter = new Character
            {
                Name = request.Name,
                Role = parsedRole,
                VideoGameId = request.VideoGameId,
                VideoGame = game,

            };
            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetCharacterById),
                new { id = newCharacter.Id },
                newCharacter);


        }


    }

}
