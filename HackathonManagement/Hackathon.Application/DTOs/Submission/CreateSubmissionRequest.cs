using System;
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Submission;

public class CreateSubmissionRequest
{
    [Required(ErrorMessage = "Mã vòng thi (RoundId) là bắt buộc.")]
    public Guid RoundId { get; set; }

    [Required(ErrorMessage = "Link kho lưu trữ (RepoUrl) là bắt buộc.")]
    [Url(ErrorMessage = "Link kho lưu trữ phải là một URL hợp lệ.")]
    [StringLength(1000, ErrorMessage = "Link không được quá 1000 ký tự.")]
    public string RepoUrl { get; set; } = string.Empty;

    [Url(ErrorMessage = "Link demo phải là một URL hợp lệ.")]
    [StringLength(1000, ErrorMessage = "Link không được quá 1000 ký tự.")]
    public string? DemoUrl { get; set; }

    [Url(ErrorMessage = "Link video phải là một URL hợp lệ.")]
    [StringLength(1000, ErrorMessage = "Link không được quá 1000 ký tự.")]
    public string? VideoUrl { get; set; }

    [StringLength(2000, ErrorMessage = "Mô tả không được vượt quá 2000 ký tự.")]
    public string? Description { get; set; }
}
