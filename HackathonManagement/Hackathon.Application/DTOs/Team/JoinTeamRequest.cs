using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Team;

public class JoinTeamRequest
{
    [Required(ErrorMessage = "Mã mời (Invite Code) là bắt buộc.")]
    public string InviteCode { get; set; } = string.Empty;
}
