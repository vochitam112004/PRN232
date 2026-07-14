namespace Hackathon.Domain.Constants;

public static class Roles
{
    public const string Organizer = "organizer";
    public const string JudgeInternal = "judge_internal";
    public const string JudgeGuest = "judge_guest";
    public const string Mentor = "mentor";
    public const string StudentFpt = "student_fpt";
    public const string StudentExternal = "student_external";

    public static readonly IReadOnlyList<string> All = new[]
    {
        Organizer, JudgeInternal, JudgeGuest, Mentor, StudentFpt, StudentExternal
    };
}
