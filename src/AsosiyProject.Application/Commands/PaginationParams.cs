namespace AsosiyProject.Application.Commands;

public class PaginationParams
{
    public int PageNumber { get; set; } = 1; // Nechanchi sahifa
    public int PageSize { get; set; } = 10;  // Har safar 10 ta post yuklanadi
}