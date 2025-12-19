using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoGameApi.Dtos;
using VideoGameApi.Services;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController(ICharacterService characterService) : ControllerBase
    {
        private readonly ICharacterService _characterService = characterService;


        [HttpGet]
        public async Task<ActionResult<List<CharacterResponseDto>>> GetCharacters()
        {
            var response = await _characterService.GetAllAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterResponseDto>> GetCharacterById(int id)
        {
            try
            {
                var character = await _characterService.GetByIdAsync(id);
                return Ok(character);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("ByGame/{gameId}")]
        public async Task<ActionResult<List<CharacterResponseDto>>> GetCharactersByGameId(int gameId)
        {
            var characters = await _characterService.GetByGameIdAsync(gameId);

            return Ok(characters);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCharacter(CharacterCreateDto request)
        {
            try
            {
                var game = await _characterService.AddAsync(request);
                return CreatedAtAction(
                    nameof(GetCharacterById),
                    new { id = game.Id },
                    game);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);

            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacter(int id , CharacterUpdateDto request)
        {
            try
            {
                await _characterService.UpdateAsync(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            try
            {
                await _characterService.SoftDeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreCharacter (int id)
        {
            try
            {
                await _characterService.RestoreAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

    }

}
