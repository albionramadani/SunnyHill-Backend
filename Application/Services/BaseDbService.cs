using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BaseDbService
    {
        public ApplicationDbContext _db;

        public BaseDbService(ApplicationDbContext db)
        {
            _db = db;
        }
    }
}
