using VideoGameApi.Dtos;

namespace VideoGameApi.Services
{
    public interface ICharacterService
    {
        Task<List<CharacterResponseDto>> GetAllAsync();
        Task<List<CharacterResponseDto>> GetByGameIdAsync(int gameId);
        Task<CharacterResponseDto?> GetByIdAsync(int id);
        Task<CharacterResponseDto> AddAsync(CharacterCreateDto request);
        Task UpdateAsync(int id , CharacterUpdateDto request);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
