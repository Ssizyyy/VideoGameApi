using VideoGameApi.Dtos;

namespace VideoGameApi.Services
{
    public interface IVideoGameService
    {
        Task<List<VideoGameResponseDto>> GetAllGamesAsync();
        Task<VideoGameResponseDto?> GetGameByIdAsync(int id);
        Task<VideoGameResponseDto> CreateGameAsync(VideoGameCreateUpdateDto request);
        Task UpdateGameAsync(int id, VideoGameCreateUpdateDto request);
        Task SoftDeleteGameAsync(int id);
    }
}
