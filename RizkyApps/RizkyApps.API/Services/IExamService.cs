using RizkyApps.Shared.DTOs;

namespace RizkyApps.API.Services
{
    public interface IExamService
    {
        Task<DtoExamResponse?> GetExamByIdAsync(int id);
        Task<IEnumerable<DtoExamResponse>> GetAllExamsAsync();
        Task<DtoExamResponse> CreateExamAsync(DtoExamCreate ExamDto);
        //Task<DtoExamResponse?> UpdateExamAsync(int id, DtoExamUpdate ExamDto);
        Task<DtoExamResponse?> UpdateExamAsync(DtoExamUpdate ExamDto);
        Task<bool> DeleteExamAsync(int id);
    }
}