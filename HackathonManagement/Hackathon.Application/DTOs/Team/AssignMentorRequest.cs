using System;
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.DTOs.Team;

public class AssignMentorRequest
{
    [Required(ErrorMessage = "MentorId là bắt buộc.")]
    public Guid MentorId { get; set; }
}
