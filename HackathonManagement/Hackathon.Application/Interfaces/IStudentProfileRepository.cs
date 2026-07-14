using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IStudentProfileRepository
{
    Task AddAsync(StudentProfile profile);
    Task<StudentProfile?> FindByUserIdAsync(Guid userId);
    Task SaveChangesAsync();
}
