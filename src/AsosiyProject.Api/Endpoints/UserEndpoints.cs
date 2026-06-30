using AsosiyProject.Application.DTOs.UserDTOs;
using AsosiyProject.Application.Interfaces.USER;
using AsosiyProject.Application.Services.SignUp.Registration;
using AsosiyProject.Application.Services.userService;
using AsosiyProject.Application.SignUp.Registration;

namespace AsosiyProject.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        // 1. Get all endpoints
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", async (IUserService userService) =>
        {
            var users = await userService.GetAllAsync();
            return Results.Ok(users);
        });


        // 2. Get user by ID
        group.MapGet("/{id}", async (Guid id, IUserService userService) =>
        {
            var user = await userService.GetByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });


        //// 3. Create a new user
        //group.MapPost("/", async (CreateUserDto userDto, IUserService userService) =>
        //{
        //    await userService.CreateAsync(userDto);
        //    return Results.Created($"/api/users/{userDto.Username}", userDto);
        //});


        // 4. Update an existing user
        group.MapPut("/", async (UpdateUserDto dto, IUserService userService) =>
        {
            var result = await userService.UpdateAsync(dto);
            return Results.Ok(result);
        });
        //.RequireAuthorization()
        //.WithTags("Users")
        //.WithName("UpdateMyProfile")
        //.WithDescription("Faqat o‘z profilingizni tahrirlashingiz mumkin");

        // Delete user endpoint.
        group.MapDelete("/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var deleted = await userService.DeleteMyAccountAsync(id);

            return Results.Ok(new { message = $"Foydalanuvchi muvaffaqiyatli o'chirildi: {deleted}" });
        });


        // 5. Get user by email
        group.MapGet("/email/{email}", async (string email, IUserService userService) =>
        {
            var user = await userService.GetByEmailAsync(email);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });


        // AuthEndpoints yoki UserEndpoints ga qo‘shing
        group.MapGet("/verify-email", async (string token, ITokenService tokenService, IUserRepository userRepo) =>
        {
            var user = await tokenService.GetUserByEmailVerificationTokenAsync(token);
            if (user == null)
                return Results.BadRequest("Havola noto‘g‘ri yoki muddati o‘tgan.");

            user.IsVerified = true;
            await userRepo.UpdateAsync(user);
            await userRepo.SaveChangesAsync();

            return Results.Ok("Email muvaffaqiyatli tasdiqlandi!" +
                              "https://asosiyproject.uz/email-verified");
        })
        .WithName("VerifyEmail")
        .WithMetadata(new EndpointNameMetadata("VerifyEmail"))
        .WithTags("Auth");


        // 6. Get user by username
        group.MapGet("/username/{username}", async (string username, IUserService userService) =>
        {
            var user = await userService.GetByUsernameAsync(username);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });


        // 7. Get user by skill
        group.MapGet("/skill/{skillName}", async (string skillName, IUserService userService) =>
        {
            var users = await userService.GetUsersBySkillAsync(skillName);

            var result = users.Select(u => new
            {
                Id = u.Id,
                Name = u.DisplayName,
                UserName = u.UserName,
                Avatar = u.DisplayPicture,
                Skills = u.Skills.Select(s => new
                {
                    Name = s.Skill?.Name,
                    Level = s.ProficiencyLevel,
                    Experience = s.YearsOfExperience
                })
            });

            return Results.Ok(result);
        });


        // 8. Get top-rated users
        group.MapGet("/top-rated/{count:int}", async (int count, IUserService userService) =>
        {
            var users = await userService.GetTopRatedUsersAsync(count);
            return Results.Ok(users);
        });


        // 9. Get users with projects
        group.MapGet("/with-projects", async (IUserService userService) =>
        {
            var users = await userService.GetUsersWithProjectsAsync();
            return Results.Ok(users);
        });


        // 10. Get check if username is taken
        group.MapGet("/is-taken/{username}", async (string username, IUserService userService) =>
        {
            var isTaken = await userService.IsUsernameTakenAsync(username);
            return Results.Ok(new { Username = username, IsTaken = isTaken });
        });
    }
}
