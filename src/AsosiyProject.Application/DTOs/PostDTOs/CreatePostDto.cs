using AsosiyProject.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AsosiyProject.Application.DTOs.PostDTOs;

public class CreatePostDto
{
    public string? Caption { get; set; }
    public IFormFile File { get; set; }
    public MediaType MediaType { get; set; }
}