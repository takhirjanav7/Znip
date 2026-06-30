namespace AsosiyProject.Application.DTOs.SkillDTOs;

public record SkillSearchResultDto(
    Guid Id,
    string Name,
    string? Category,
    string? IconUrl,
    int UsersCount,
    bool IsAddedByCurrentUser       // foydalanuvchi allaqachon qo‘shganmi?
);