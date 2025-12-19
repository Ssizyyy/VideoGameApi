using VideoGameApi.Data;
using VideoGameApi.Dtos;
using VideoGameApi.Models;
using VideoGameApi.Repositories;

namespace VideoGameApi.Services
{
    public class CharacterService(IRepository<Character> characterRepo ,
        IRepository<VideoGame> gameRepo,
        VideoGameDbContext context) : ICharacterService
    {
        private readonly IRepository<Character> _characterRepo = characterRepo;
        private readonly IRepository<VideoGame> _gameRepo = gameRepo;
        private readonly VideoGameDbContext _dbContext = context;
        private CharacterResponseDto MapToDto(Character character)
        {
            return new CharacterResponseDto
            {
                Id = character.Id,
                Name = character.Name,
                Role = character.Role.ToString(),
                VideoGameId = character.VideoGameId,
                VideoGameTitle = character.VideoGame?.Title ?? string.Empty
            };
        }
        public async Task<List<CharacterResponseDto>> GetAllAsync()
        {
            var characters = await _characterRepo.GetAllAsync(c => c.VideoGame!);
            return characters.Select(MapToDto).ToList();
        }
        public async Task<CharacterResponseDto?> GetByIdAsync(int id)
        {
            var character= await _characterRepo.GetByIdAsync(id);
            if (character == null)
            {
                throw new KeyNotFoundException($"Character with ID {id} not found.");
            }
            return MapToDto(character);
        }
        public async Task<List<CharacterResponseDto>> GetByGameIdAsync(int gameId)
        {
            var allCharacters = await _characterRepo.GetAllAsync();
            return allCharacters.Where(c => c.VideoGameId == gameId)
                                .Select(MapToDto)
                                .ToList();
        }
        private async Task<CharacterResponseDto?> GetByIdIncludingDeletedAsync(int id)
        {
            var character = await _characterRepo.GetByIdIncludingDeletedAsync(id);
            if (character == null)
            {
                throw new KeyNotFoundException($"Character with ID {id} not found.");
            }
            return MapToDto(character);
        }
        public async Task<CharacterResponseDto> AddAsync(CharacterCreateDto request)
        {
            var game = await _gameRepo.GetByIdAsync(request.VideoGameId);
            if (game == null)
            {
                throw new KeyNotFoundException($"Video game with ID {request.VideoGameId} not found.");
            }
            var newCharacter = new Character
            {
                Name = request.Name,
                Role = request.Role,
                VideoGameId = request.VideoGameId
            };
            await _characterRepo.AddAsync(newCharacter);
            await _dbContext.SaveChangesAsync();
            return MapToDto(newCharacter);
        }
        public async Task UpdateAsync(int id, CharacterUpdateDto request)
        {
            var character = await _characterRepo.GetByIdAsync(id);
            if (character == null)
            {
                throw new KeyNotFoundException($"Character with ID {id} not found.");
            }
            if (!String.IsNullOrEmpty(request.Name)) character.Name = request.Name;
            if (request.Role.HasValue) character.Role = request.Role.Value;
            await _dbContext.SaveChangesAsync();

        }
        public async Task SoftDeleteAsync(int id)
        {
            var character = await _characterRepo.GetByIdAsync(id);
            if (character == null)
            {
                throw new KeyNotFoundException($"Character with ID {id} not found.");
            }
            _characterRepo.SoftDelete(character);
            await _dbContext.SaveChangesAsync();
        }
        public async Task RestoreAsync(int id)
        {
            var character = await _characterRepo.GetByIdIncludingDeletedAsync(id);
            if (character == null)
            {
                throw new KeyNotFoundException($"Character with ID {id} not found.");
            }
            if (!character.IsDeleted)
            {
                throw new InvalidOperationException($"Character with ID {id} is not deleted.");
            }
            _characterRepo.Restore(character);
            await _dbContext.SaveChangesAsync();
        }
    }
}
