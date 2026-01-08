using Microsoft.AspNetCore.Http.HttpResults;
using RizkyApps.Shared.DTOs;
using RizkyApps.API.Services;

namespace RizkyApps.API.Endpoints
{
    public static class ExamEndpoints
    {
        public static void RegisterExamEndpoints(this WebApplication app)
        {
            var ExamGroup = app.MapGroup("/api/exam")
                .WithTags("Exam");

            // GET /api/Exams
            ExamGroup.MapGet("/", async (IExamService ExamService) =>
            {
                var Exams = await ExamService.GetAllExamsAsync();
                return Results.Ok(Exams);
            })
            .WithSummary("Get all Exams")
            .Produces<List<DtoExamResponse>>(StatusCodes.Status200OK)
            .WithDescription("Returns a All of exam objects."); // ✅ Use this;

            // GET /api/Exams/{id}
            ExamGroup.MapGet("/{id}", async (int id, IExamService ExamService) =>
            {
                var Exam = await ExamService.GetExamByIdAsync(id);
                return Exam is not null ? Results.Ok(Exam) : Results.NotFound();
            })
            .WithSummary("Get Exam by ID")
            .Produces<DtoExamResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Returns 1 exam objects."); // ✅ Use this;

            // POST /api/Exams
            ExamGroup.MapPost("/", async (DtoExamCreate ExamDto, IExamService ExamService) =>
            {
                var Exam = await ExamService.CreateExamAsync(ExamDto);
                return Results.Created($"/api/Exams/{Exam.Id}", Exam);
            })
            .WithSummary("Create a new Exam")
            .Produces<DtoExamResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

            // PUT /api/Exams
            ExamGroup.MapPut("/", async (DtoExamUpdate ExamDto, IExamService ExamService) =>
            {
                var Exam = await ExamService.UpdateExamAsync(ExamDto);
                return Exam is not null ? Results.Ok(Exam) : Results.NotFound();
            })
            .WithSummary("Update an existing Exam")
            .Produces<DtoExamResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // DELETE /api/Exams/{id}
            ExamGroup.MapDelete("/{id}", async (int id, IExamService ExamService) =>
            {
                var deleted = await ExamService.DeleteExamAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithSummary("Delete a Exam")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}