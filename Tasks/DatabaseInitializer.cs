using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public class DatabaseInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        //private  readonly UserManager<ApplicationUser> _userManager;

        public DatabaseInitializer(ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }

        public async Task Init()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.EnsureCreatedAsync();
        }
    }
}
