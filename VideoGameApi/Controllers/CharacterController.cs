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
        public async Task<ActionResult<List<CharacterResponseDto>>> GetCharacters()
        {
            var response = await _context.Characters
                .Select(c => new CharacterResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Role = c.Role.ToString(),
                    VideoGameId = c.VideoGameId,
                    VideoGameTitle = c.VideoGame!.Title ?? "N/A"
                })
                .ToListAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterResponseDto>> GetCharacterById(int id)
        {
            var character = await _context.Characters
                .Include(c => c.VideoGame)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (character == null)
                return NotFound("Character Not Found");

            var dto = new CharacterResponseDto
            {
                Id = character.Id,
                Name = character.Name,
                Role = character.Role.ToString(),
                VideoGameId = character.VideoGameId,
                VideoGameTitle = character.VideoGame?.Title ?? "N/A"
            };
            return Ok(dto);
        }


        //[HttpGet("ByGame/{gameId}")]
        //public async Task<ActionResult<List<Character>>> GetCharactersByGameId(int gameId)
        //{
        //    var characters = await _context.Characters
        //        .Where(c => c.VideoGameId == gameId)
        //        .ToListAsync();
        //    return Ok(characters);
        //}

        [HttpPost]
        public async Task<ActionResult> CreateCharacter(CharacterCreateDto request)
        {
            var game = await _context.VideoGames.FindAsync(request.VideoGameId);
            if (game == null)
                    return NotFound("Game Not Found");

            var newCharacter = new Character
            {
                Name = request.Name,
                Role = request.Role,
                VideoGameId = request.VideoGameId,
                VideoGame = game,

            };

            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            var responseDto = new CharacterResponseDto
            {
                Id = newCharacter.Id,
                Name = newCharacter.Name,
                Role = newCharacter.Role.ToString(),
                VideoGameId = newCharacter.VideoGameId,
                VideoGameTitle = game.Title ?? "N/A"
            };


            return CreatedAtAction(
                nameof(GetCharacterById),
                new { id = newCharacter.Id },
                responseDto);


        }


    }

}
