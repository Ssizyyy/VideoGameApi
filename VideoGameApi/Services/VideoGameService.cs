using Microsoft.AspNetCore.Http.HttpResults;
using VideoGameApi.Data;
using VideoGameApi.Dtos;
using VideoGameApi.Models;
using VideoGameApi.Repositories;

namespace VideoGameApi.Services
{
    public class VideoGameService : IVideoGameService
    {
        private readonly IRepository<VideoGame> _gameRepo;
        private readonly IRepository<Character> _characterRepo;
        private readonly VideoGameDbContext _dbContext;
        public VideoGameService(IRepository<VideoGame> gameRepo,
            IRepository<Character> characterRepo,
            VideoGameDbContext context)
        {
            _gameRepo = gameRepo;
            _characterRepo = characterRepo;
            _dbContext = context;
        }
        public async Task<List<VideoGameResponseDto>> GetAllGamesAsync()
        {
            var games = await _gameRepo.GetAllAsync(g => g.Characters);
            return games.Select(g => new VideoGameResponseDto
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
                }).ToList()

            }).ToList();
        }
        public async Task<VideoGameResponseDto?> GetGameByIdAsync(int id)
        {
            var game = await _gameRepo.GetByIdAsync(id,g=>g.Characters);
            if (game == null) return null;
            return new VideoGameResponseDto
            {
                Id = game.Id,
                Title = game.Title,
                Developer = game.Developer,
                Platform = game.Platform,
                Publisher = game.Publisher,
                Characters = game.Characters.Select(c=> new CharacterResponseDto
                {
                    Id= c.Id,
                    Name = c.Name,
                    Role = c.Role.ToString()
                }).ToList()
            };

        }
        public async Task<VideoGameResponseDto> CreateGameAsync(VideoGameCreateUpdateDto request)
        {
            var newGame = new VideoGame
            {
                Title = request.Title,
                Developer = request.Developer,
                Platform = request.Platform,
                Publisher = request.Publisher
            };
            await _gameRepo.AddAsync(newGame);
            await _dbContext.SaveChangesAsync();

            return new VideoGameResponseDto
            {
                Id = newGame.Id,
                Title = request.Title,
                Developer = request.Developer,
                Platform = request.Platform,
                Publisher = request.Publisher
            };
        }
        public async Task UpdateGameAsync(int id, VideoGameCreateUpdateDto request)
        {
            var game = await _gameRepo.GetByIdAsync(id);
            if (game == null) return;
            if (!string.IsNullOrEmpty(request.Title)) game.Title = request.Title;
            if (!string.IsNullOrEmpty(request.Developer)) game.Developer = request.Developer;
            if (!string.IsNullOrEmpty(request.Platform)) game.Platform = request.Platform;
            if (!string.IsNullOrEmpty(request.Publisher)) game.Publisher = request.Publisher;

            await _dbContext.SaveChangesAsync();
        }
        public async Task SoftDeleteGameAsync(int id)
        {
            var game = await _gameRepo.GetByIdAsync(id, g=>g.Characters);
            if (game == null) return;
            if (game.Characters != null && game.Characters.Any())
            {
                throw new InvalidOperationException("Cannot delete a game that has active characters.");
            }
            _gameRepo.SoftDelete(game);
            await _dbContext.SaveChangesAsync();
        }
        public async Task RestoreGameAsync(int id)
        {
            var deletedGame = await _gameRepo.GetByIdIncludingDeletedAsync(id);
            if (deletedGame == null)
            {
                throw new KeyNotFoundException($"No deleted game found with ID {id}");
            }
            if (!deletedGame.IsDeleted)
            {
                throw new InvalidOperationException($"Game with ID {id} is not deleted.");
            }
            _gameRepo.Restore(deletedGame);
            await _dbContext.SaveChangesAsync();
        }
    }
}