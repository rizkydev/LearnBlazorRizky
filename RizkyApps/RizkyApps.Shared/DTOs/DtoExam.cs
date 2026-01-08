namespace RizkyApps.Shared.DTOs
{
    public record DtoExamResponse(
        int Id,
        string Title,
        bool IsRandomQuestion,
        bool IsRandomAnswer,
        bool IsMultipleAttempt,
        string Note
    );

    public record DtoExamCreate(
        string Title,
        bool IsRandomQuestion,
        bool IsRandomAnswer,
        bool IsMultipleAttempt,
        string Note,
        string CreatedBy
    );

    public record DtoExamUpdate(
        int Id,
        string Title,
        bool IsRandomQuestion,
        bool IsRandomAnswer,
        bool IsMultipleAttempt,
        string Note,
        string ModifiedBy
    );
}
