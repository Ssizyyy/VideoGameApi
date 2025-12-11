using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VideoGameApi.Data;
using VideoGameApi.Dtos;
using VideoGameApi.Models;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController(VideoGameDbContext context) : ControllerBase
    {
        private readonly VideoGameDbContext _context = context;


        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetVideoGames()
        {
            var games = await _context.VideoGames
                .Include(g => g.Characters)
                .Select(g => new VideoGameResponseDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    Developer = g.Developer,
                    Platform = g.Platform,
                    Publisher = g.Publisher,
                    Characters = g.Characters.Select(c => new CharacterResponseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Role = c.Role.ToString(),
                        VideoGameId = c.VideoGameId,
                        VideoGameTitle = g.Title
                    }).ToList()
                }).ToListAsync();
            return Ok(games);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {
            var game = await _context.VideoGames
                .Include(g=>g.Characters)
                .FirstOrDefaultAsync(g=> g.Id == id);
            if (game is null)
            {
                return NotFound();
            }

            var responseDto = new VideoGameResponseDto
            {
                Id = game.Id,
                Title = game.Title,
                Developer = game.Developer,
                Platform = game.Platform,
                Publisher = game.Publisher,
                Characters = game.Characters.Select(c => new CharacterResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Role = c.Role.ToString(),
                    VideoGameId = c.VideoGameId,
                    VideoGameTitle = game.Title
                }).ToList()
            };

            return Ok(responseDto);
        }
        [HttpPost]
        public async Task<ActionResult> AddVideoGame(VideoGameCreateUpdateDto request)
        {
            if (request == null)
                return BadRequest();

            var newGame = new VideoGame
            {
                Title = request.Title,
                Developer = request.Developer,
                Platform = request.Platform,
                Publisher = request.Publisher
            };

            _context.VideoGames.Add(newGame);
            await _context.SaveChangesAsync();


            var requestDto = new VideoGameResponseDto
            {
                Id = newGame.Id,
                Title = newGame.Title,
                Developer = newGame.Developer,
                Platform = newGame.Platform,
                Publisher = newGame.Publisher,
                Characters = new List<CharacterResponseDto>()
            };


            return CreatedAtAction(
                nameof(GetVideoGameById),
                new { id = newGame.Id },
                requestDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoGame(int id ,VideoGameCreateUpdateDto request)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
                return NotFound();
            game.Title = request.Title;
            game.Platform = request.Platform;
            game.Developer = request.Developer;
            game.Publisher = request.Publisher;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGame (int id)
        {
            var game = await _context.VideoGames.FindAsync(id);
            if (game is null)
                return NotFound();
            _context.VideoGames.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
