using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class StudentProfileRepository : IStudentProfileRepository
{
    private readonly ApplicationDbContext _db;

    public StudentProfileRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(StudentProfile profile)
        => await _db.StudentProfiles.AddAsync(profile);

    public Task<StudentProfile?> FindByUserIdAsync(Guid userId)
        => _db.StudentProfiles.FirstOrDefaultAsync(s => s.UserId == userId);

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
