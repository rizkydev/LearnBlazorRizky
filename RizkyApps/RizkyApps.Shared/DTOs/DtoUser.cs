namespace RizkyApps.Shared.DTOs
{
    public record DtoUserResponse(
        long Id,
        string? Name,
        string Email,
        string? Phone,
        DateTime? Birthday
    );

    public record DtoUserCreate(
        string Username,
        string Password,
        string? Name,
        string Email,
        string? Phone,
        DateTime? Birthday
    );

    public record DtoUserUpdate(
        long Id,
        string Username,
        string Password,
        string? Name,
        string Email,
        string? Phone,
        DateTime? Birthday
    );

}
