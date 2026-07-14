using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Domain.Enums
{
    public enum AuditAction
    {
        ScoreSubmitted,
        ScoreUpdated,
        TeamDisqualified,
        SubmissionDisqualified,
        RoundResultFinalized,
        AwardGranted
    }
}
