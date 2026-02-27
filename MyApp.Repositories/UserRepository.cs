using Microsoft.EntityFrameworkCore;
using MyApp.Core.Interfaces;
using MyApp.Models;
using System;
using MyApp.Infrastructure;

namespace MyApp.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}
