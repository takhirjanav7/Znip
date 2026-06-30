namespace AsosiyProject.Application.Extensions;

public static class DateTimeExtensions
{
    public static string ToTimeAgo(this DateTime dateTime)
    {
        var now = DateTime.UtcNow;
        var diff = now - dateTime.ToUniversalTime();

        return diff.TotalSeconds switch
        {
            < 60 => "hozirgina",
            < 120 => "1 daqiqa oldin",
            < 3600 => $"{(int)diff.TotalMinutes} daqiqa oldin",
            < 7200 => "1 soat oldin",
            < 86400 => $"{(int)diff.TotalHours} soat oldin",
            < 172800 => "kecha",
            < 2592000 => $"{(int)diff.TotalDays} kun oldin",
            < 5184000 => "1 oy oldin",
            < 31536000 => $"{(int)(diff.TotalDays / 30)} oy oldin",
            < 63072000 => "1 yil oldin",
            _ => $"{(int)(diff.TotalDays / 365)} yil oldin"
        };
    }

    // Agar local vaqt kerak bo‘lsa (foydalanuvchi sozlamasiga qarab)
    public static string ToTimeAgo(this DateTime? dateTime)
        => dateTime?.ToTimeAgo() ?? "noma'lum";
}