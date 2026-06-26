using System;
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Team;

public class CreateTeamRequest
{
    [Required(ErrorMessage = "Tên đội thi là bắt buộc.")]
    [StringLength(100, ErrorMessage = "Tên đội thi không được vượt quá 100 ký tự.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Hạng mục (CategoryId) là bắt buộc.")]
    public Guid CategoryId { get; set; }
}
