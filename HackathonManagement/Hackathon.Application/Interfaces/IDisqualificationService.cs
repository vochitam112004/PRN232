using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Application.Interfaces
{
     public interface IDisqualificationService
     {
        Task DisqualifyTeamAsync(Guid teamId, Guid performedBy, string reason, string? ipAddress = null);
        Task DisqualifySubmissionAsync(Guid submissionId, Guid performedBy, string reason, string? ipAddress = null);
    }
}
