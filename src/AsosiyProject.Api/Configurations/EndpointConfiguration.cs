using AsosiyProject.Api.Endpoints;

namespace AsosiyProject.Api.Configurations;

public static class EndpointConfiguration
{
    public static void ConfigureEndpoints(this WebApplication app)
    {

        //// Comment endpointlarini qo'shish
        //app.MapCommentEndpoints();

        // Chat endpointlarini qo'shish
        app.MapChatEndpoints();

        // Follow endpointlarini qo'shish
        app.MapFollowEndpoints();

        //// Like endpointlarini qo'shish
        //app.MapLikeEndpoints();

        //// Xabar endpointlarini qo'shish
        //app.MapMessageEndpoints();

        // Malumotlar endpointlarini qo'shish
        app.MapNotificationEndpoints();

        // Postlar endpointlarini qo'shish
        app.MapPostEndpoints();

        //// Loyihalar endpointlarini qo'shish
        //app.MapProjectEndpoints();



        // Ro'yhatdan o'tish endpointlarini qo'shish
        app.MapAuthEndpoints();

        // Qobiliyatlar endpointlarini qo'shish
        app.MapSkillEndpoints();

        // Foydalanuvchilar endpointlarini qo'shish
        app.MapUserEndpoints();
    }
}