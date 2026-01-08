using Microsoft.EntityFrameworkCore;
using RizkyApps.API.Data;
using RizkyApps.Shared.DTOs;
using RizkyApps.API.Models;

namespace RizkyApps.API.Services
{
    public class ExamService : IExamService
    {
        private readonly ApplicationDbContext _context;

        public ExamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DtoExamResponse>> GetAllExamsAsync()
        {
            var Exams = await _context.Exams
                .OrderBy(p => p.Id)
                .ToListAsync();

            return Exams.Select(p => MapToDto(p));
        }

        public async Task<DtoExamResponse?> GetExamByIdAsync(int id)
        {
            var Exam = await _context.Exams.FindAsync(id);
            return Exam != null ? MapToDto(Exam) : null;
        }

        public async Task<DtoExamResponse> CreateExamAsync(DtoExamCreate ExamDto)
        {
            var Exam = new Exam
            {
                Title = ExamDto.Title,
                IsRandomQuestion = ExamDto.IsRandomQuestion,
                IsRandomAnswer = ExamDto.IsRandomAnswer,
                IsMultipleAttempt = ExamDto.IsMultipleAttempt,
                Note = ExamDto.Note,
                CreatedBy = ExamDto.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = ExamDto.CreatedBy,
                ModifiedDate = DateTime.UtcNow
            };

            _context.Exams.Add(Exam);
            await _context.SaveChangesAsync();

            return MapToDto(Exam);
        }

        //public async Task<DtoExamResponse?> UpdateExamAsync(int id, DtoExamUpdate ExamDto)
        public async Task<DtoExamResponse?> UpdateExamAsync(DtoExamUpdate ExamDto)
        {
            var Exam = await _context.Exams.FindAsync(ExamDto.Id);
            if (Exam == null) return null;

            // Update only provided fields using C# 10's null-coalescing assignment
            Exam.Title = ExamDto.Title ?? Exam.Title;
            Exam.IsRandomQuestion = ExamDto.IsRandomQuestion;
            Exam.IsRandomAnswer = ExamDto.IsRandomAnswer;
            Exam.IsMultipleAttempt = ExamDto.IsMultipleAttempt;
            Exam.Note = ExamDto.Note;
            Exam.ModifiedBy = ExamDto.ModifiedBy;
            Exam.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(Exam);
        }

        public async Task<bool> DeleteExamAsync(int id)
        {
            var Exam = await _context.Exams.FindAsync(id);
            if (Exam == null) return false;

            _context.Exams.Remove(Exam);
            await _context.SaveChangesAsync();
            return true;
        }

        private static DtoExamResponse MapToDto(Exam Exam) =>
            new DtoExamResponse(
                Exam.Id,
                Exam.Title,
                Exam.IsRandomQuestion,
                Exam.IsRandomAnswer,
                Exam.IsMultipleAttempt,
                Exam.Note
            );
    }
}