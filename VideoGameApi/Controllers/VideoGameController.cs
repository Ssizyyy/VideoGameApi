using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VideoGameApi.Data;
using VideoGameApi.Dtos;
using VideoGameApi.Models;
using VideoGameApi.Services;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController(IVideoGameService videoGameService) : ControllerBase
    {
        private readonly IVideoGameService _videoGameService = videoGameService;


        [HttpGet]
        public async Task<ActionResult<List<VideoGameResponseDto>>> GetVideoGames()
        {
            var games = await _videoGameService.GetAllGamesAsync();
            return Ok(games);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<VideoGameResponseDto>> GetVideoGameById(int id)
        {
            var game = await _videoGameService.GetGameByIdAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }
        [HttpPost]
        public async Task<ActionResult> AddVideoGame(VideoGameCreateUpdateDto request)
        {
            if (request == null)
                return BadRequest();
            var createdGame = await _videoGameService.CreateGameAsync(request);
            return CreatedAtAction(
                nameof(GetVideoGameById),
                new { id = createdGame.Id },
                createdGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoGame(int id ,VideoGameCreateUpdateDto request)
        {
            var existingGame = await _videoGameService.GetGameByIdAsync(id);
            if (existingGame == null) return NotFound();

            await _videoGameService.UpdateGameAsync(id, request);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGame (int id)
        {
            var existingGame = await _videoGameService.GetGameByIdAsync(id);
            if(existingGame == null) return NotFound();
            try
            {
                await _videoGameService.SoftDeleteGameAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            
            return NoContent();
        }
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreVideoGame(int id)
        {
            try
            {
                await _videoGameService.RestoreGameAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
