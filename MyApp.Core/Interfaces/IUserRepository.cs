using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Models;

namespace MyApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        //对IRepository扩展的
        Task<User?> GetByEmailAsync(string email);
    }

}
