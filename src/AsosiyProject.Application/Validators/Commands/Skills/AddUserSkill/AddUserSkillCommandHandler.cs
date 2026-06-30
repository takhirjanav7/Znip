using AsosiyProject.Application.Commands;
using AsosiyProject.Application.Common.Interfaces;
using AsosiyProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AsosiyProject.Application.Validators.Commands.Skills.AddUserSkill;

public class AddUserSkillCommandHandler : IRequestHandler<AddUserSkillCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AddUserSkillCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AddUserSkillCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();

        // Skill mavjudligini tekshirish
        var skillExists = await _context.Skills
            .AnyAsync(s => s.Id == request.SkillId, ct);

        if (!skillExists)
            throw new KeyNotFoundException("Bunday skill topilmadi!");

        // Allaqachon qo‘shilganmi?
        var alreadyAdded = await _context.UserSkills
            .AnyAsync(us => us.UserId == userId && us.SkillId == request.SkillId, ct);

        if (alreadyAdded)
            throw new InvalidOperationException("Bu skill allaqachon qo‘shilgan!");

        // Qo‘shish
        var userSkill = new UserSkill
        {
            UserId = userId,
            SkillId = request.SkillId,
            ProficiencyLevel = request.ProficiencyLevel,
            AddedAt = DateTime.UtcNow
        };

        _context.UserSkills.Add(userSkill);
        await _context.SaveChangesAsync(ct);

        return Unit.Value;
    }
}